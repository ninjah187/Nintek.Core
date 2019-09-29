using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintek.Core.Domain
{
    public abstract class DomainObject
    {
        protected abstract IEnumerable<object> GetEqualityMembers();

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // this == other
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = (DomainObject)obj;

            return GetFlattenedEqualityMembers().SequenceEqual(other.GetFlattenedEqualityMembers());
        }

        public static bool operator ==(DomainObject left, DomainObject right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator !=(DomainObject left, DomainObject right)
        {
            return !(left == right);
        }

        // calculating hash code:
        // http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode

        const int Prime1 = 17;
        const int Prime2 = 23;

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = Prime1;
                foreach (var member in GetFlattenedEqualityMembers())
                {
                    hash = hash * Prime2 + (member?.GetHashCode() ?? 0);
                }
                return hash;
            }
        }

        IEnumerable<object> GetFlattenedEqualityMembers()
        {
            foreach (var member in GetEqualityMembers())
            {
                if (!(member is IEnumerable collection))
                {
                    yield return member;
                }
                else
                {
                    // what about collection of collections? [][]?
                    foreach (var collectionMember in collection)
                    {
                        yield return collectionMember;
                    }
                }
            }
        }
    }
}
