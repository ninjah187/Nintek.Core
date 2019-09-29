using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintek.Core.Collections
{
    public class TrackingList<T> : ITrackingCollection<T>, IReadOnlyList<T>, IList<T>
    {
        public IEnumerable<T> Added => _defaultOrAdded
            .Where(tracked => tracked.State == TrackedItemState.Added)
            .Select(tracked => tracked.Item);

        public IEnumerable<T> Removed => _removed.Value;

        readonly List<TrackedItem<T>> _defaultOrAdded;
        readonly Lazy<List<T>> _removed = new Lazy<List<T>>(() => new List<T>());

        public TrackingList()
        {
            _defaultOrAdded = new List<TrackedItem<T>>();
        }

        public TrackingList(IEnumerable<T> collection)
        {
            _defaultOrAdded = new List<TrackedItem<T>>(collection.Select(item => new TrackedItem<T>(item, TrackedItemState.Default)));
        }

        public int Count => _defaultOrAdded.Count;

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get => _defaultOrAdded[index].Item;
            set => _defaultOrAdded[index] = new TrackedItem<T>(value, TrackedItemState.Added); // TODO: shouldn't be State.Updated?
        }

        public void Add(T item)
        {
            _defaultOrAdded.Add(new TrackedItem<T>(item, TrackedItemState.Added));
        }

        public void Clear()
        {
            for (var i = 0; i < _defaultOrAdded.Count; i++)
            {
                var tracked = _defaultOrAdded[i];
                _defaultOrAdded.RemoveAt(i);
                _removed.Value.Add(tracked.Item);
            }
        }

        public bool Contains(T item)
        {
            return _defaultOrAdded.Any(tracked => tracked.Item.Equals(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (var i = 0; i < _defaultOrAdded.Count; i++)
            {
                array[arrayIndex++] = _defaultOrAdded[i].Item;
            }
        }

        public int IndexOf(T item)
        {
            for (var i = 0; i < _defaultOrAdded.Count; i++)
            {
                var other = _defaultOrAdded[i];
                if (EqualityComparer<T>.Default.Equals(other.Item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            _defaultOrAdded.Insert(index, new TrackedItem<T>(item, TrackedItemState.Added));
        }

        public bool Remove(T item)
        {
            for (var i = 0; i < _defaultOrAdded.Count; i++)
            {
                var tracked = _defaultOrAdded[i];
                if (tracked.Item.Equals(item))
                {
                    _defaultOrAdded.RemoveAt(i);
                    if (tracked.State == TrackedItemState.Default)
                    {
                        _removed.Value.Add(tracked.Item);
                    }
                    return true;
                }
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index >= _defaultOrAdded.Count)
            {
                return;
            }
            var removedItem = _defaultOrAdded[index].Item;
            _defaultOrAdded.RemoveAt(index);
            _removed.Value.Add(removedItem);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _defaultOrAdded
                .Select(trackedItem => trackedItem.Item)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
