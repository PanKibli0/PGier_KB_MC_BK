using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("Event Pool")]
    public List<EventData> eventPool;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public EventData getRandomEvent()
    {
        if (eventPool == null || eventPool.Count == 0)
        {
            Debug.LogError("EventPool is empty!");
            return null;
        }

        return eventPool[Random.Range(0, eventPool.Count)];
    }
}