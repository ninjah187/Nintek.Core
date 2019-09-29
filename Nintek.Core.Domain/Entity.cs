using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain
{
    public abstract class Entity : DomainObject
    {
    }

    public abstract class Entity<TIdentifier> : Entity
        where TIdentifier : ValueObject
    {
        public TIdentifier Id { get; }

        protected Entity()
        {
        }

        protected Entity(TIdentifier id)
        {
            Id = id;
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return Id;
        }
    }
}
