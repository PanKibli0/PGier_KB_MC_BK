using UnityEngine;
using UnityEngine.UI;

public class NodeButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    [SerializeField] private Image visitedOverlay;

    private BaseNode node;


    public void init(BaseNode node, int currentFloor)
    {
        this.node = node;

        bool isActive = node.isUnlocked && !node.isVisited && node.gridPosition.y == currentFloor;
        button.interactable = isActive;

        string path = node.getIconPath();
        if (!string.IsNullOrEmpty(path))
            icon.sprite = Resources.Load<Sprite>(path);

        if (node.isVisited && !string.IsNullOrEmpty(node.visitedIconPath))
        {
            Sprite visitedSprite = Resources.Load<Sprite>(node.visitedIconPath);
            if (visitedSprite != null)
                visitedOverlay.sprite = visitedSprite;
            visitedOverlay.gameObject.SetActive(true);
        }
    }


    public void onClick()
    {
        GameManager.Instance.currentMap.currentFloor = node.gridPosition.y + 1;
        node.execute();
    }
}