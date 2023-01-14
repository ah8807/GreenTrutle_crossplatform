using System.Collections.Generic;
using System;
using System.Collections;

namespace GreenTrutle_crossplatform;

public class EventManager
{
    private Dictionary<string, EventHandler<Dictionary<string, object>>> eventDictionary;
    private ArrayList eventList = new ArrayList();
    public EventManager()
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
            eventList.Add(listener);
        }
        else
        {
            thisEvent += listener;
            eventDictionary.Add(eventName, thisEvent);
            eventList.Add(listener);
        }
    }
    public void Unsubscribe(string eventName, EventHandler<Dictionary<string, object>> listener)
    {
        EventHandler<Dictionary<string, object>> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
            eventList.Remove(listener);
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

    public void clearAll(string eventName)
    {
        EventHandler<Dictionary<string, object>> thisEvent;


        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            foreach (EventHandler<Dictionary<string, object>> even in eventList)
            {
                thisEvent -= even;
                eventDictionary[eventName] = thisEvent;
            } 
        }
    }
}
