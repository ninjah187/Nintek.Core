using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Events
{
    public class EventQueue : IReadOnlyCollection<IEvent>
    {
        public int Count => _events.Count;

        readonly List<IEvent> _events = new List<IEvent>();

        public void Add(IEvent @event)
        {
            _events.Add(@event);
        }

        public IEnumerable<IEvent> Flush()
        {
            foreach (var @event in this)
            {
                yield return @event;
            }
            _events.Clear();
        }

        public void Capture(IEventEmitter emitter)
        {
            var flush = emitter.Events.Flush();
            _events.AddRange(flush);
        }

        public IEnumerator<IEvent> GetEnumerator()
            => _events.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _events.GetEnumerator();
    }
}
