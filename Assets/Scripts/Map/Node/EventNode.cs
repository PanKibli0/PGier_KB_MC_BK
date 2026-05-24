using UnityEngine;
using UnityEngine.SceneManagement;

public class EventNode : BaseNode
{
    public EventData eventData;

    public override string getIconPath()
    {
        return "Icons_map/event";
    }

    public override void execute()
    {
        GameManager.Instance.currentEvent = eventData;
        GameManager.Instance.currentMapNode = this;
        SceneManager.LoadScene("EventScene");
    }
}