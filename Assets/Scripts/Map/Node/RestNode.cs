using UnityEngine;

public class RestNode : BaseNode
{
    public override void execute()
    {
        GameManager.Instance.currentMapNode = this;
        UnityEngine.SceneManagement.SceneManager.LoadScene("RestScene");
    }

    public override string getIconPath()
    {
        return "Icons_map/odpoczynek";
    }
}
