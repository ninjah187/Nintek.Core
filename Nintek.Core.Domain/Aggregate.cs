using Nintek.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain
{
    public abstract class Aggregate<TRoot> : IEventEmitter
        where TRoot : Entity
    {
        public EventQueue Events { get; } = new EventQueue();

        public TRoot Root { get; }

        protected Aggregate(TRoot root)
        {
            Root = root;
        }
    }
}
