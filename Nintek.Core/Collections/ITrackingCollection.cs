using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Collections
{
    public interface ITrackingCollection<T> : IEnumerable<T>, IReadOnlyCollection<T>, ICollection<T>
    {
        IEnumerable<T> Added { get; }
        IEnumerable<T> Removed { get; }
    }
}
