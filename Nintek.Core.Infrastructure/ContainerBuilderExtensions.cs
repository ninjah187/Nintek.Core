using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Infrastructure
{
    public static class ContainerBuilderExtensions
    {
        public static void LoadNintekAppCore(this ContainerBuilder builder, params string[] eventAssemblies)
        {
            var module = new NintekCoreModule(eventAssemblies);
            builder.RegisterModule(module);
        }
    }
}
