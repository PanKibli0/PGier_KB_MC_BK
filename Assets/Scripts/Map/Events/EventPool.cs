using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventsPool", menuName = "Events/EventPool")]
public class EventsPool : ScriptableObject
{
    public List<EventData> events = new List<EventData>();

    public EventData GetRandomEvent()
    {
        if (events == null || events.Count == 0)
            return null;

        return events[Random.Range(0, events.Count)];
    }
}