using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Common
{
    public class Required<TValue>
    {
        public TValue Value { get; }

        public Required(TValue value)
        {
            if (value == null)
            {
                throw new DomainException($"Required value of type {typeof(TValue).FullName} is null.");
            }
            Value = value;
        }

        public static implicit operator TValue(Required<TValue> required)
        {
            return required.Value;
        }

        public static implicit operator Required<TValue>(TValue value)
        {
            return new Required<TValue>(value);
        }

        public static bool operator ==(Required<TValue> value1, TValue value2)
        {
            return value1.Value.Equals(value2);
        }

        public static bool operator !=(Required<TValue> value1, TValue value2)
        {
            return !value1.Value.Equals(value2);
        }

        public static bool operator <(Required<TValue> value1, Required<TValue> value2)
        {
            return Comparer<TValue>.Default.Compare(value1, value2) < 0;
        }

        public static bool operator >(Required<TValue> value1, Required<TValue> value2)
        {
            return Comparer<TValue>.Default.Compare(value1, value2) > 0;
        }

        public override string ToString()
        {
            return Value?.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Required<TValue> required && Value.Equals(required.Value);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}
