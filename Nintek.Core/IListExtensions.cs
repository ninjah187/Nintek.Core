using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core
{
    public static class IListExtensions
    {
        // TODO: should method jumping only one index be called Pairwise?
        public static IEnumerable<(T current, T next)> Pairwise<T>(this IList<T> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var current = list[i];
                var next = i + 1 < list.Count ? list[i + 1] : default(T);
                yield return (current, next);
            }
        }
    }
}
