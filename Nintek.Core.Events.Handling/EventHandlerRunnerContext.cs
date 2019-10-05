using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Events.Handling
{
    public class EventHandlerRunnerContext
    {
        public string EventHandlerName { get; }
        public AsyncEventHandlerRunner Runner { get; }
        public bool EventualConsistent { get; }

        public EventHandlerRunnerContext(
            string eventHandlerName,
            AsyncEventHandlerRunner runner,
            bool eventualConsistent)
        {
            EventHandlerName = eventHandlerName;
            Runner = runner;
            EventualConsistent = eventualConsistent;
        }
    }
}
