using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventChoice
{
    public string buttonText;
    public string resultText;
    public List<EventEffect> effects = new List<EventEffect>();
}