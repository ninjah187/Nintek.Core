using Microsoft.Extensions.Logging;
using Nintek.Core.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nintek.Core.Events.Handling
{
    public class EventualConsistentWorker : IDisposable
    {
        readonly IDependencyScope _rootScope;
        readonly EventHandlerRunnerCache _eventHandlerRunnerCache;
        readonly ILogger<EventualConsistentWorker> _logger;

        // TODO: benchmark limits for eventsToConsume bounded capacity and concurrent consumers count

        // change to ConcurrentQueue?
        readonly BlockingCollection<EventualConsistentEvent> _eventsToConsume = new BlockingCollection<EventualConsistentEvent>(2048);
        readonly SemaphoreSlim _consumersSemaphore = new SemaphoreSlim(4);
        readonly int _handleAttemptsLimit = 3;

        public EventualConsistentWorker(
            IDependencyScope rootScope,
            EventHandlerRunnerCache eventHandlerRunnerCache,
            ILogger<EventualConsistentWorker> logger)
        {
            _rootScope = rootScope;
            _eventHandlerRunnerCache = eventHandlerRunnerCache;
            _logger = logger;
        }

        public void Dispose()
        {
            _eventsToConsume.CompleteAdding();
            _eventsToConsume.Dispose();
        }

        public void Process(EventualConsistentEvent eventualConsistent)
        {
            _eventsToConsume.Add(eventualConsistent);
        }

        public async Task StartConsumption()
        {
            _ = Task.Run(ProcessEventsFromStore);
            await Task.Factory.StartNew(Consume, TaskCreationOptions.LongRunning);
        }

        async Task Consume()
        {
            foreach (var eventToConsume in Feed())
            {
                await _consumersSemaphore.WaitAsync();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await Consume(eventToConsume);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, $"Exception on consuming event {eventToConsume.Event.Id}.");
                        throw;
                    }
                    finally
                    {
                        _consumersSemaphore.Release();
                    }
                });
            }
        }

        IEnumerable<EventualConsistentEvent> Feed()
        {
            while (!_eventsToConsume.IsCompleted)
            {
                EventualConsistentEvent eventToConsume = null;
                try
                {
                    // BlockingCollection blocks if _eventsToConsume.Count == 0.
                    eventToConsume = _eventsToConsume.Take();
                }
                catch (InvalidOperationException)
                {
                    // IOE means that Take() was called on a completed collection.
                    // Some other thread can call CompleteAdding after we pass the
                    // IsCompleted check but before we call Take. 
                    yield break;
                }
                yield return eventToConsume;
            }
        }

        async Task Consume(EventualConsistentEvent eventToConsume)
        {
            using (var scope = _rootScope.BeginScope())
            {
                try
                {
                    eventToConsume.CountHandleAttempt();
                    await Consume(scope, eventToConsume);
                }
                catch (EventHandlerRunnerContextNotFoundException exception)
                {
                    eventToConsume.Error(exception.ToString());
                    await UpdateEvent(scope, eventToConsume);
                    throw;
                }
                catch (Exception exception)
                {
                    if (eventToConsume.HandleAttempts < _handleAttemptsLimit)
                    {
                        Process(eventToConsume);
                    }
                    else
                    {
                        eventToConsume.Fail(exception.ToString());
                        await UpdateEvent(scope, eventToConsume);
                    }
                    throw;
                }
            }
        }

        async Task Consume(IDependencyScope scope, EventualConsistentEvent eventualConsistent)
        {
            var unitOfWork = scope.Resolve<IUnitOfWork>();
            var repository = scope.Resolve<IEventRepository>();
            var eventType = eventualConsistent.Event.GetType();
            var context = _eventHandlerRunnerCache
                .Get(eventType) // TODO: change .Get parameter to event's full type name? try to reduce GetType() and reflection calls during events handling
                .FirstOrDefault(x => x.EventHandlerName == eventualConsistent.EventHandlerName);
            if (context == null)
            {
                throw new EventHandlerRunnerContextNotFoundException(eventType.FullName, eventualConsistent.EventHandlerName);
            }
            await context.Runner(scope, eventualConsistent.Event);
            await repository.Remove(eventualConsistent.Event);
            await unitOfWork.Commit();
        }

        async Task ProcessEventsFromStore()
        {
            using (var scope = _rootScope.BeginScope())
            {
                var repo = scope.Resolve<IEventRepository>();
                var events = await repo.GetAllProcessables();
                foreach (var @event in events)
                {
                    Process(@event);
                }
            }
        }

        static async Task UpdateEvent(IDependencyScope scope, EventualConsistentEvent eventualConsistent)
        {
            var repo = scope.Resolve<IEventRepository>();
            await repo.Update(eventualConsistent);
            await Commit(scope);
        }

        static async Task Commit(IDependencyScope scope)
        {
            var unitOfWork = scope.Resolve<IUnitOfWork>();
            await unitOfWork.Commit();
        }
    }
}
