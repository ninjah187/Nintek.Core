using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Common
{
    public class Double : ValueObject
    {
        public double Value { get; }

        public Double(double value)
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
