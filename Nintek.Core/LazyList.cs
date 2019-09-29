using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core
{
    public class LazyList<T> : IEnumerable<T>, IReadOnlyCollection<T>
    {
        public int Count => Items.Count;

        List<T> Items => _items ?? (_items = new List<T>(_sourceProvider()));
        List<T> _items;

        readonly Func<IEnumerable<T>> _sourceProvider;

        public LazyList(IEnumerable<T> source)
            : this(() => source)
        {
        }

        public LazyList(Func<IEnumerable<T>> sourceProvider)
        {
            _sourceProvider = sourceProvider;
        }

        public void Add(T item)
        {
            Items.Add(item);
        }

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
