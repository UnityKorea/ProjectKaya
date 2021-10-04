using System;
using System.Collections.Generic;

namespace UTK.Manager
{
    public class UTKEventManager
    {
        private static readonly Dictionary<Type, List<IUtkEventListenerBase>> SubscriberDictionary;

        static UTKEventManager()
        {
            SubscriberDictionary = new Dictionary<Type, List<IUtkEventListenerBase>>();
        }

        public static void AddListener<TUtkEvent>(IUtkEventListener<TUtkEvent> listener) where TUtkEvent : struct
        {
            Type eventType = typeof(TUtkEvent);

            if (!SubscriberDictionary.ContainsKey(eventType))
                SubscriberDictionary[eventType] = new List<IUtkEventListenerBase>();
            
            if(!SubscriptionExists(eventType,listener))
                SubscriberDictionary[eventType].Add(listener);
        }

        public static void RemoveListener<TUtkEvent>(IUtkEventListener<TUtkEvent> listener ) where TUtkEvent : struct
        {
            Type eventType = typeof(TUtkEvent);

            if (!SubscriberDictionary.ContainsKey(eventType))
            {
                return;
            }

            List<IUtkEventListenerBase> subscriberList = SubscriberDictionary[eventType];

            for (int i = 0; i < subscriberList.Count; i++)
            {
                if (subscriberList[i] == listener)
                {
                    subscriberList.Remove(subscriberList[i]);
                    if (subscriberList.Count == 0)
                        SubscriberDictionary.Remove(eventType);
                }
                
            }
        }

        public static void TriggerEvent<TUtkEvent>(TUtkEvent newEvent) where TUtkEvent : struct
        {
            List<IUtkEventListenerBase> list;
            if(!SubscriberDictionary.TryGetValue(typeof(TUtkEvent), out list))
                return;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i] as IUtkEventListener<TUtkEvent>; 
                if(item != null) item.OnUtkEvent(newEvent);
            }
        }
        

        private static bool SubscriptionExists(Type type, IUtkEventListenerBase receiver)
        {
            List<IUtkEventListenerBase> receivers;
            if (!SubscriberDictionary.TryGetValue(type, out receivers)) return false;
            var exists = false;
            for (int i = 0; i < receivers.Count; i++)
            {
                if (receivers[i] == receiver)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }
    }
    public static class EventRegister
    {
        public delegate void Delegate<T>(T eventType);

        
        public static void UtkEventStartListening<TEventType>( this IUtkEventListener<TEventType> caller) where TEventType : struct
        {
            UTKEventManager.AddListener(caller);
        }

        public static void UtkEventStopListening<TEventType>( this IUtkEventListener<TEventType> caller) where TEventType : struct
        {
            UTKEventManager.RemoveListener(caller);
        }
            
    }
    
    
    
    public interface IUtkEventListenerBase
    {
    };

    public interface IUtkEventListener<T> : IUtkEventListenerBase
    {
        void OnUtkEvent(T eventType);
    }
}