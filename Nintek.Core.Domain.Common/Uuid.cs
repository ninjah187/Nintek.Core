using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Common
{
    public class Uuid : ValueObject
    {
        public Guid Value { get; }

        public Uuid(Guid value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return Value;
        }
    }
}
