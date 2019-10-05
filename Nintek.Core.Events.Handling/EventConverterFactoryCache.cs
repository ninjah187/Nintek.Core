using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintek.Core.Events.Handling
{
    public class EventConverterFactoryCache
    {
        readonly Dictionary<string, Func<IDependencyScope, IEventConverter>> _cache = new Dictionary<string, Func<IDependencyScope, IEventConverter>>();

        public void Populate(Type[] eventsAssembly)
        {
            var eventConverterContexts = eventsAssembly
                .Where(type => type.GetInterfaces().Any(@interface => @interface == typeof(IEvent)))
                .Select(eventType => new
                {
                    EventName = eventType.FullName,
                    ConverterFactory = ConstructEventConverterFactory(eventsAssembly, eventType)
                })
                .Where(context => context.ConverterFactory != null);
            foreach (var context in eventConverterContexts)
            {
                _cache.Add(context.EventName, context.ConverterFactory);
            }
        }

        public Func<IDependencyScope, IEventConverter> Get(string eventName)
        {
            return _cache[eventName];
        }

        static Func<IDependencyScope, IEventConverter> ConstructEventConverterFactory(Type[] eventsAssembly, Type eventType)
        {
            var baseEventConverterType = typeof(EventConverter<>).MakeGenericType(eventType);
            return eventsAssembly
                .Where(type => type.IsDerivedFrom(baseEventConverterType))
                .Select(converterType => EventConverterFactory(converterType))
                .SingleOrDefault();
        }

        static Func<IDependencyScope, IEventConverter> EventConverterFactory(Type converterType)
        {
            var constructor = converterType.GetConstructors().Single();
            var parameterTypes = constructor
                .GetParameters()
                .Select(parameter => parameter.ParameterType)
                .ToArray();
            return scope =>
            {
                var parameters = parameterTypes
                    .Select(parameterType => scope.Resolve(parameterType))
                    .ToArray();
                return (IEventConverter)constructor.Invoke(parameters);
            };
        }
    }

    static class TypeExtensions
    {
        public static bool IsDerivedFrom(this Type type, Type derivedFrom)
        {
            while (type != null)
            {
                if (type == derivedFrom)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }
    }
}
