using System.Collections.Generic;
using UnityEngine.Events;

namespace Utilities
{
    public static class EventBus
    {
        public enum EventType
        {
            TAKE_DAMAGE,
            CHANGE_BLOCK,
            GAME_START,
            GAME_END,
            GAME_PAUSE
        }

        public static Dictionary<EventType, UnityEvent> Events = new Dictionary<EventType, UnityEvent>();

        public static void Subscribe(EventType type, UnityAction listener)
        {
            UnityEvent thisEvent;
            if (Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Events.Add(type, thisEvent);
            }
        }
    

        public static void Unsubscribe(EventType type, UnityAction listener)
        {
            UnityEvent thisEvent;
            if (Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void Publish(EventType type)
        {
            UnityEvent thisEvent;
            if (Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}