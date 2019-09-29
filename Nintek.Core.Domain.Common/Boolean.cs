using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Common
{
    public class Boolean : ValueObject
    {
        public bool Value { get; }

        public Boolean(bool value)
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
