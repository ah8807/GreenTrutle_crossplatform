using System.Collections.Generic;
using System;
using System.Collections;

namespace GreenTrutle_crossplatform;

public class EventManager
{
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManager();
            }
            return instance;
        }
    }
    private Dictionary<string, EventHandler<Dictionary<string, object>>> eventDictionary;
    private EventManager()
    {
        eventDictionary = new Dictionary<string, EventHandler<Dictionary<string, object>>>();
    }
    public void Subscribe(string eventName, EventHandler<Dictionary<string, object>> listener)
    {
        EventHandler<Dictionary<string, object>> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            eventDictionary.Add(eventName, thisEvent);
        }
    }
    public void Unsubscribe(string eventName, EventHandler<Dictionary<string, object>> listener)
    {
        EventHandler<Dictionary<string, object>> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }
    }

    public void Trigger(string eventName, Object? sender ,Dictionary<string, object> message)
    {
        EventHandler<Dictionary<string, object>> thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(sender, message);
        }
    }

    public void clearAll()
    {
        eventDictionary.Clear();
    }
    public void ClearListeners(string eventType)
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary.Remove(eventType);
        }
    }
}
