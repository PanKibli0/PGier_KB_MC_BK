using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Event")]
public class EventData : ScriptableObject
{
    public string eventName;

    [TextArea(5, 10)]
    public string description;

    public Sprite illustration;

    public List<EventChoice> choices;
}