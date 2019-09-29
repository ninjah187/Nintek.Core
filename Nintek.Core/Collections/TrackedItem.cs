using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Collections
{
    public class TrackedItem<T>
    {
        public T Item { get; }
        public TrackedItemState State { get; }

        public TrackedItem(T item, TrackedItemState state)
        {
            Item = item;
            State = state;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TrackedItem<T>;
            return other != null &&
                   EqualityComparer<T>.Default.Equals(Item, other.Item) &&
                   State == other.State;
        }

        public override int GetHashCode()
        {
            var hashCode = -477813876;
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Item);
            hashCode = hashCode * -1521134295 + State.GetHashCode();
            return hashCode;
        }
    }
}
