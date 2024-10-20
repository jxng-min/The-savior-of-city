using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _EventBus
{
    public class GameEventBus
    {
        private static readonly IDictionary<GameEventType, UnityEvent>
            m_events = new Dictionary<GameEventType, UnityEvent>();

        public static void Subscribe(GameEventType event_type, UnityAction listener)
        {
            UnityEvent this_event;

            if(m_events.TryGetValue(event_type, out this_event))
                this_event.AddListener(listener);
            else
            {
                this_event = new UnityEvent();
                this_event.AddListener(listener);
                m_events.Add(event_type, this_event);
            }
        }

        public static void Unsubscribe(GameEventType event_type, UnityAction listener)
        {
            UnityEvent this_event;

            if(m_events.TryGetValue(event_type, out this_event))
                this_event.RemoveListener(listener);
        }

        public static void Publish(GameEventType event_type)
        {
            UnityEvent this_event;

            if(m_events.TryGetValue(event_type, out this_event))
                this_event.Invoke();
        }
    }
}