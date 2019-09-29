using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Common
{
    public class Integer : ValueObject
    {
        public int Value { get; }

        public Integer(int value)
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
