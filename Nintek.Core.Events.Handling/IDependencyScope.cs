using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Events.Handling
{
    public interface IDependencyScope : IDisposable
    {
        IDependencyScope BeginScope();
        object Resolve(Type type);
    }
}
