using Microsoft.Extensions.Logging;
using Nintek.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Events.Handling
{
    public class EventDispatcher
    {
        readonly EventualConsistentWorker _eventualConsistentWorker;
        readonly IEventRepository _repository;
        readonly EventHandlerRunnerCache _cache;
        readonly IDependencyScope _rootScope;
        readonly ILogger<EventDispatcher> _logger;

        readonly Queue<EventualConsistentEvent> _eventualConsistentEvents = new Queue<EventualConsistentEvent>(); // TODO: lazy?

        public EventDispatcher(
            EventualConsistentWorker eventualConsistentWorker,
            IUnitOfWork unitOfWork,
            IEventRepository repository,
            EventHandlerRunnerCache cache,
            IDependencyScope rootScope,
            ILogger<EventDispatcher> logger)
        {
            _eventualConsistentWorker = eventualConsistentWorker;
            _repository = repository;
            _cache = cache;
            _rootScope = rootScope;
            _logger = logger;

            unitOfWork.Commited += uow =>
            {
                while (_eventualConsistentEvents.Count != 0)
                {
                    var eventualConsistent = _eventualConsistentEvents.Dequeue();
                    _eventualConsistentWorker.Process(eventualConsistent);
                }
            };
        }

        public async Task Dispatch(IEnumerable<IEventEmitter> emitters)
        {
            foreach (var @event in emitters.SelectMany(emitter => emitter.Events.Flush()))
            {
                await DispatchEvent(@event);
            }
        }

        async Task DispatchEvent(IEvent @event)
        {
            var contexts = _cache.Get(@event.GetType());
            foreach (var context in contexts)
            {
                if (context.EventualConsistent)
                {
                    var eventualConsistent = new EventualConsistentEvent(@event, context.EventHandlerName);
                    await _repository.Add(eventualConsistent);
                    _eventualConsistentEvents.Enqueue(eventualConsistent);
                }
                else
                {
                    await RunEventHandler(@event, context);
                }
            }
        }

        async Task RunEventHandler(IEvent @event, EventHandlerRunnerContext context)
        {
            _logger.LogInformation($"Event '{@event.Id}' handling by '{context.EventHandlerName}'.");
            await context.Runner(_rootScope, @event);
            _logger.LogInformation($"Event '{@event.Id}' handled by '{context.EventHandlerName}'.");
        }
    }
}
