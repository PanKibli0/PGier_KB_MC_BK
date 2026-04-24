using UnityEngine;
using UnityEngine.UI;

public class NodeButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    [SerializeField] private GameObject visitedOverlay;

    private BaseNode node;


    public void init(BaseNode node, int currentFloor)
    {
        this.node = node;

        bool isActive = node.isUnlocked && !node.isVisited && node.gridPosition.y == currentFloor;
        button.interactable = isActive;
        visitedOverlay.SetActive(node.isVisited);

        string path = node.getIconPath();
        if (!string.IsNullOrEmpty(path))
            icon.sprite = Resources.Load<Sprite>(path);
    }


    public void onClick()
    {
        GameManager.Instance.currentMap.currentFloor = node.gridPosition.y + 1;
        node.execute();
    }
}