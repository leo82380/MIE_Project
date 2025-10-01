using System;
using System.Collections.Generic;

namespace MIE.Runtime.EventSystem.Core
{
    public static class EventHandler
    {
        private static Dictionary<Type, Delegate> eventTable = new Dictionary<Type, Delegate>();

        public static void Subscribe<T>(Action<T> listener) where T : struct, IEvent
        {
            var type = typeof(T);
            if (eventTable.ContainsKey(type))
            {
                eventTable[type] = Delegate.Combine(eventTable[type], listener);
            }
            else
            {
                eventTable[type] = listener;
            }
        }

        public static void Unsubscribe<T>(Action<T> listener) where T : struct, IEvent
        {
            var type = typeof(T);
            if (eventTable.ContainsKey(type))
            {
                var currentDel = eventTable[type];
                currentDel = Delegate.Remove(currentDel, listener);

                if (currentDel == null)
                {
                    eventTable.Remove(type);
                }
                else
                {
                    eventTable[type] = currentDel;
                }
            }
        }

        public static void TriggerEvent<T>(T eventData) where T : struct, IEvent
        {
            var type = typeof(T);
            if (eventTable.ContainsKey(type))
            {
                var callback = eventTable[type] as Action<T>;
                callback?.Invoke(eventData);
            }
        }

        public static void TriggerEvent<T>() where T : IEvent, new()
        {
            var type = typeof(T);
            if (eventTable.TryGetValue(type, out var del))
            {
                (del as Action<T>)?.Invoke(new T());
            }
        }

        public static void Clear()
        {
            eventTable.Clear();
        }
    }

    public interface IEvent
    {
    }
}