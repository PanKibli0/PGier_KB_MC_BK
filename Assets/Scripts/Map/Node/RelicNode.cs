using UnityEngine;
using UnityEngine.SceneManagement;

public class RelicNode : BaseNode
{
    public override void execute()
    {
        GameManager.Instance.currentMapNode = this;
        SceneManager.LoadScene("RelicScene");
    }

    public override string getIconPath()
    {
        return "Icons_map/skarb"; 
    }
}