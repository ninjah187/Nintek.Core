using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Events.Handling
{
    public class EventData
    {
        public EventData(
            int id,
            string eventName,
            string handlerName,
            EventualConsistentState state,
            int processAttempts,
            string payload)
        {
            Id = id;
            EventName = eventName;
            HandlerName = handlerName;
            State = state;
            ProcessAttempts = processAttempts;
            Payload = payload;
        }

        public int Id { get; }
        public string EventName { get; }
        public string HandlerName { get; }
        public EventualConsistentState State { get; }
        public int ProcessAttempts { get; }
        public string Payload { get; }
    }
}
