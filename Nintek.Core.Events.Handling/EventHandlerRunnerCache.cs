using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Events.Handling
{
    public class EventHandlerRunnerCache
    {
        // TODO: for better performance use IL.Emit() or compiled lambda expressions

        readonly Dictionary<Type, EventHandlerRunnerContext[]> _cache = new Dictionary<Type, EventHandlerRunnerContext[]>();

        public void Populate(Type[] eventTypesSpace)
        {
            foreach (var eventType in GetEventTypes(eventTypesSpace))
            {
                AddCacheItem(eventTypesSpace, eventType);
            }
        }

        public EventHandlerRunnerContext[] Get(Type eventType)
        {
            return _cache[eventType];
        }

        void AddCacheItem(Type[] eventTypesSpace, Type eventType)
        {
            var contexts = GetEventHandlerRunnerContexts(eventTypesSpace, eventType);
            _cache.Add(eventType, contexts);
        }

        static Type[] GetEventTypes(Type[] eventTypesSpace)
        {
            return eventTypesSpace
                .Where(type => type.GetInterfaces().Any(@interface => @interface == typeof(IEvent)))
                .ToArray();
        }

        static EventHandlerRunnerContext[] GetEventHandlerRunnerContexts(Type[] eventTypesSpace, Type eventType)
        {
            var handlerInterfaceType = typeof(IAsyncEventHandler<>).MakeGenericType(eventType);
            return eventTypesSpace
                .Where(type => type.GetInterfaces().Any(@interface => @interface == handlerInterfaceType))
                .Select(handlerType => GetEventHandlerRunnerContext(handlerType))
                .ToArray();
        }

        static EventHandlerRunnerContext GetEventHandlerRunnerContext(Type handlerType)
        {
            var constructor = handlerType.GetConstructors().Single();
            var parameterTypes = constructor
                .GetParameters()
                .Select(parameter => parameter.ParameterType)
                .ToArray();
            var factory = GetHandlerFactory(constructor, parameterTypes);
            var runner = GetAsyncEventHandlerRunner(handlerType, factory);
            var eventualConsistent = handlerType.GetCustomAttribute<EventualConsistentAttribute>() != null;
            return new EventHandlerRunnerContext(handlerType.FullName, runner, eventualConsistent);
        }

        static EventHandlerFactory GetHandlerFactory(ConstructorInfo constructor, Type[] parameterTypes)
        {
            return scope =>
            {
                var parameters = parameterTypes
                    .Select(parameterType => scope.Resolve(parameterType))
                    .ToArray();
                return constructor.Invoke(parameters);
            };
        }

        // TODO: consider which version choose: await or return Task.

        //static AsyncEventHandlerRunner GetAsyncEventHandlerRunner(Type handlerType, EventHandlerFactory handlerFactory)
        //{
        //    var handleMethod = handlerType.GetMethod(nameof(IAsyncEventHandler<IEvent>.Handle));
        //    return (scope, @event) =>
        //    {
        //        var task = handleMethod.Invoke(handlerFactory(scope), new[] { @event });
        //        return (Task) task;
        //    };
        //}

        static AsyncEventHandlerRunner GetAsyncEventHandlerRunner(Type handlerType, EventHandlerFactory handlerFactory)
        {
            var handleMethod = handlerType.GetMethod(nameof(IAsyncEventHandler<IEvent>.Handle));
            return async (scope, @event) =>
            {
                var task = handleMethod.Invoke(handlerFactory(scope), new[] { @event });
                await (Task)task;
            };
        }
    }
}
