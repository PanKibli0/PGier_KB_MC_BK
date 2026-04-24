using UnityEngine;
using UnityEngine.UI;

public class NodeButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    [SerializeField] private GameObject visitedOverlay;

    private BaseNode node;

    public void init(BaseNode node)
    {
        this.node = node;
        button.interactable = node.isUnlocked;
        visitedOverlay.SetActive(node.isVisited);

        string iconPath = node.getIconPath();
        if (!string.IsNullOrEmpty(iconPath))
        {
            Sprite sprite = Resources.Load<Sprite>(iconPath);
            if (sprite != null)
                icon.sprite = sprite;
        }
    }


    public void onClick()
    {
        if (node.isUnlocked && !node.isVisited)
            node.execute();
    }
}