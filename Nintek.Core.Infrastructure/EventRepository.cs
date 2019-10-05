using Nintek.Core.Data;
using Nintek.Core.Data.Dapper;
using Nintek.Core.Events;
using Nintek.Core.Events.Handling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Infrastructure
{
    public class EventRepository : Repository, IEventRepository
    {
        readonly IDependencyScope _scope;
        readonly IJsonConverter _jsonConverter;
        readonly EventConverterFactoryCache _converterFactoryCache;

        public EventRepository(
            IUnitOfWork unitOfWork,
            IDependencyScope scope,
            IJsonConverter jsonConverter,
            EventConverterFactoryCache converterFactoryCache)
            : base(unitOfWork)
        {
            _scope = scope;
            _jsonConverter = jsonConverter;
            _converterFactoryCache = converterFactoryCache;
        }

        public async Task Add(EventualConsistentEvent eventualConsistent)
        {
            // TODO: don't call GetType() and get name from some cached context?
            var @event = eventualConsistent.Event;
            var eventHandlerName = eventualConsistent.EventHandlerName;
            var eventName = @event.GetType().FullName;
            var customConverter = CreateConverter(eventName);
            var payload = customConverter == null
                ? _jsonConverter.Serialize(@event)
                : customConverter.Convert(@event);
            await UnitOfWork.ExecuteAsync(
                "INSERT INTO events VALUES (DEFAULT, @eventName, @eventHandlerName, 0, 0, @payload)",
                new { eventName, eventHandlerName, payload });
        }

        public async Task Remove(IEvent @event)
        {
            await UnitOfWork.ExecuteAsync("DELETE FROM events WHERE id = @Id", @event);
        }

        public async Task<EventualConsistentEvent[]> Take(int count = 1)
        {
            var data = await UnitOfWork.QueryAsync<EventData>(
                "SELECT id, eventName, handlerName, state, processAttempts, payload FROM events ORDER BY id ASC LIMIT @count",
                new { count });
            return await Get(data);
        }

        public async Task<EventualConsistentEvent[]> GetAllProcessables()
        {
            var data = await UnitOfWork.QueryAsync<EventData>(
                "SELECT id, eventName, handlerName, state, processAttempts, payload FROM events WHERE state = 0");
            return await Get(data);
        }

        async Task<EventualConsistentEvent[]> Get(IEnumerable<EventData> data)
        {
            var tasks = data.Select(async eventData =>
            {
                var customConverter = CreateConverter(eventData.EventName);
                // TODO: don't call Type.GetType() and get event's Type from some cached context?
                // TODO: review code of this class and dependent classes (EventConverter etc)
                var @event = customConverter == null
                    ? (IEvent)_jsonConverter.Deserialize(eventData.Payload, Type.GetType(eventData.EventName))
                    : await customConverter.ConvertBack(eventData.Id, eventData.Payload);
                return new EventualConsistentEvent(@event, eventData.HandlerName);
            });
            return await TaskEx.OneAtATime(tasks);
        }

        public async Task Update(EventualConsistentEvent eventualConsistent)
        {
            await UnitOfWork.ExecuteAsync(
                "UPDATE  events " +
                "SET     state = @state, " +
                "        processAttempts = @processAttempts," +
                "        errorMessage = @errorMessage " +
                "WHERE   id = @id ",
                new
                {
                    id = eventualConsistent.Event.Id,
                    state = eventualConsistent.State,
                    processAttempts = eventualConsistent.ProcessAttempts,
                    errorMessage = eventualConsistent.ErrorMessage
                });
        }

        IEventConverter CreateConverter(string eventName)
        {
            return _converterFactoryCache.Get(eventName)?.Invoke(_scope);
        }
    }
}
