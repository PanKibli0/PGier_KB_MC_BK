using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopNode : BaseNode
{
    public override void execute()
    {
        GameManager.Instance.currentMapNode = this;
        SceneManager.LoadScene("ShopScene");
    }

    public override string getIconPath()
    {
        return "Icons_map/sklep";
    }
}