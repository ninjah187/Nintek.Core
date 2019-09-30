using Nintek.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain
{
    public abstract class AggregateRoot<TIdentifier> : Entity<TIdentifier>, IEventEmitter
        where TIdentifier : ValueObject
    {
        public EventQueue Events { get; } = new EventQueue();

        protected AggregateRoot(TIdentifier id) : base(id)
        {
        }
    }
}
