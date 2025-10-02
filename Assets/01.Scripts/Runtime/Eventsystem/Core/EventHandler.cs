using System;
using System.Collections.Generic;

namespace MIE.Runtime.EventSystem.Core
{
    // 이벤트들을 관리하는 클래스
    public static class EventHandler
    {
        private static Dictionary<Type, Delegate> eventTable = new Dictionary<Type, Delegate>();

        /// <summary>
        /// 이벤트 구독 메서드
        /// </summary>
        /// <typeparam name="T">구독할 이벤트의 타입</typeparam>
        /// <param name="listener">이벤트가 발생했을 때 호출될 콜백 메서드</param>
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

        /// <summary>
        /// 이벤트 구독 해제 메서드
        /// </summary>
        /// <typeparam name="T">구독 해제할 이벤트의 타입</typeparam>
        /// <param name="listener">구독 해제할 콜백 메서드</param>
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

        /// <summary>
        /// 이벤트 트리거 메서드
        /// </summary>
        /// <typeparam name="T">발현시킬 이벤트의 타입</typeparam>
        /// <param name="eventData">이벤트 데이터</param>
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