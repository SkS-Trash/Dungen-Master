using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Subscribers.EventBusSystem
{
    public static class EventBus
    {
        private static readonly ConcurrentDictionary<Type, SubscribersList<IGlobalSubscriber>> Subscribers = new();
        private static readonly ConcurrentDictionary<Type, List<Type>> CachedSubscriberTypes = new();
        private static readonly Type IGlobalSubscriberType = typeof(IGlobalSubscriber);

        public static void Subscribe(IGlobalSubscriber subscriber)
        {
            var subscriberTypes = GetSubscriberTypes(subscriber);
            foreach (var list in subscriberTypes.Select(type =>
                         Subscribers.GetOrAdd(type, _ => new SubscribersList<IGlobalSubscriber>())))
            {
                list.Add(subscriber);
            }
        }

        public static void Unsubscribe(IGlobalSubscriber subscriber)
        {
            var subscriberTypes = GetSubscriberTypes(subscriber);
            foreach (var type in subscriberTypes)
            {
                if (Subscribers.TryGetValue(type, out var list))
                {
                    list.Remove(subscriber);
                }
            }
        }

        public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action)
            where TSubscriber : class, IGlobalSubscriber
        {
            if (Subscribers.TryGetValue(typeof(TSubscriber), out var list))
            {
                list.Execute(subscriber => action((TSubscriber)subscriber));
            }
        }

        private static List<Type> GetSubscriberTypes(IGlobalSubscriber subscriber)
        {
            var type = subscriber.GetType();
            if (CachedSubscriberTypes.TryGetValue(type, out var types))
            {
                return types;
            }

            types = type
                .GetInterfaces()
                .Where(i => IGlobalSubscriberType.IsAssignableFrom(i))
                .Where(i => i != IGlobalSubscriberType)
                .ToList();

            CachedSubscriberTypes[type] = types;

            return types;
        }
    }
}