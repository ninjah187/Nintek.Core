using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Events
{
    public abstract class Event : IEvent
    {
        public int Id { get; }

        protected Event()
        {
        }

        protected Event(int id)
        {
            Id = id;
        }
    }
}
