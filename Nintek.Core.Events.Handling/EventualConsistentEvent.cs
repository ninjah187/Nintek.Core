using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Events.Handling
{
    public class EventualConsistentEvent
    {
        public IEvent Event { get; }
        public string EventHandlerName { get; }
        public EventualConsistentState State { get; private set; }
        public int ProcessAttempts { get; private set; }
        public string ErrorMessage { get; private set; }
        public int HandleAttempts { get; private set; }

        public EventualConsistentEvent(
            IEvent @event,
            string eventHandlerName,
            EventualConsistentState state = EventualConsistentState.Processable)
        {
            Event = @event;
            EventHandlerName = eventHandlerName;
            State = state;
        }

        public EventualConsistentEvent(
            IEvent @event,
            string eventHandlerName,
            EventualConsistentState state,
            int processAttempts,
            string error)
            : this(@event, eventHandlerName, state)
        {
            ProcessAttempts = processAttempts;
            ErrorMessage = error;
        }


        public void CountHandleAttempt()
        {
            HandleAttempts++;
        }

        public void Fail(string errorMessage)
        {
            State = EventualConsistentState.Failed;
            ErrorMessage = errorMessage;
            ProcessAttempts++;
        }

        public void Error(string errorMessage)
        {
            State = EventualConsistentState.Error;
            ErrorMessage = errorMessage;
            ProcessAttempts++;
        }
    }
}
