using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Events.Handling
{
    public static class IScopeExtensions
    {
        public static T Resolve<T>(this IDependencyScope scope)
        {
            return (T) scope.Resolve(typeof(T));
        }
    }
}
