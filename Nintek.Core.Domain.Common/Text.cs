using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Common
{
    public abstract class Text : ValueObject
    {
        public string Value { get; protected set; }

        protected Text(
            string value,
            int minLength = 0,
            int maxLength = int.MaxValue)
        {
            value = value ?? "";
            if (value.Length < minLength)
            {
                throw new DomainException($"String's '{value}' length exceeded limit of minimum length: {minLength}.");
            }
            if (value.Length > maxLength)
            {
                throw new DomainException($"String's '{value}' length exceeded limit of maximum length: {maxLength}.");
            }
            Value = value;
        }

        protected Text(string value, int expectedLength)
            : this(value, expectedLength, expectedLength)
        {
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(Text obj)
        {
            return obj.Value;
        }
    }
}
