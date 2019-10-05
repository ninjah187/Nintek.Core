using Autofac;
using Nintek.Core.Events.Handling;
using System;

namespace Nintek.Core.Infrastructure
{
    public class AutofacDependencyScope : IDependencyScope
    {
        readonly ILifetimeScope _scope;

        public AutofacDependencyScope(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public IDependencyScope BeginScope() => new AutofacDependencyScope(_scope.BeginLifetimeScope());

        public void Dispose() => _scope.Dispose();

        public object Resolve(Type type) => _scope.Resolve(type);
    }
}
