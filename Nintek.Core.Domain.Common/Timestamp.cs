using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Common
{
    public abstract class Timestamp : ValueObject
    {
        public DateTime Value { get; }

        public Timestamp(DateTime value)
            : this(value, DateTime.MinValue, DateTime.MaxValue)
        {
        }

        public Timestamp(DateTime value, DateTime min, DateTime max)
        {
            if (value < min)
            {
                throw new DomainException($"DateTime {value} is lesser than allowed minimum {min}.");
            }
            if (value > max)
            {
                throw new DomainException($"DateTime {value} is greater than allowe maximum {max}.");
            }
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
