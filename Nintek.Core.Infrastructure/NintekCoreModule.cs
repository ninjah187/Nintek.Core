using Autofac;
using Nintek.Core.Data;
using Nintek.Core.Data.Npgsql;
using Nintek.Core.Events.Handling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assembly = System.Reflection.Assembly;

namespace Nintek.Core.Infrastructure
{
    public class NintekCoreModule : Module
    {
        readonly string[] _eventAssemblies;

        public NintekCoreModule(string[] eventAssemblies)
        {
            _eventAssemblies = eventAssemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var eventTypes = _eventAssemblies
                .SelectMany(assemblyName => LoadTypes(assemblyName))
                .ToArray();

            builder
                .Register(_ =>
                {
                    var cache = new EventHandlerRunnerCache();
                    cache.Populate(eventTypes);
                    return cache;
                })
                .SingleInstance();
            builder
                .Register(_ =>
                {
                    var cache = new EventConverterFactoryCache();
                    cache.Populate(eventTypes);
                    return cache;
                })
                .SingleInstance();
            builder
                .RegisterType<EventualConsistentWorker>()
                .SingleInstance();
            builder
                .RegisterType<NewtonsoftJsonConverter>()
                .As<IJsonConverter>()
                .SingleInstance();
            builder
                .RegisterType<Clock>()
                .SingleInstance();
            builder
                .RegisterType<EventDispatcher>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<EventRepository>()
                .As<IEventRepository>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<NpgsqlDbConnector>()
                .As<IDbConnector>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<NpgsqlUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<AutofacDependencyScope>()
                .As<IDependencyScope>()
                .InstancePerLifetimeScope();
        }

        static Type[] LoadTypes(string assemblyName)
        {
            return Assembly
                .Load(assemblyName)
                .GetTypes();
        }
    }
}
