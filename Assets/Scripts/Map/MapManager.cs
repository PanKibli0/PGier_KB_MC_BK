using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject nodeButtonPrefab;
    private float offsetX = 200f;
    private float offsetY = 200f;


    void Start()
    {
        displayMap();
    }


    private void displayMap()
    {
        List<BaseNode> nodes = GameManager.Instance.currentMap.nodes;
        int currentFloor = GameManager.Instance.currentMap.currentFloor;

        foreach (var node in nodes)
        {
            GameObject btnObj = Instantiate(nodeButtonPrefab, contentContainer);
            RectTransform rect = btnObj.GetComponent<RectTransform>();
            rect.anchoredPosition = getNodePosition(node);

            NodeButton btn = btnObj.GetComponent<NodeButton>();
            btn.init(node, currentFloor);
        }

        adjustContentSize();
    }


    private Vector2 getNodePosition(BaseNode node)
    {
        return new Vector2(
            node.gridPosition.x * offsetX,
            node.gridPosition.y * offsetY + 20f
            );
    }

    private void adjustContentSize()
    {
        List<BaseNode> nodes = GameManager.Instance.currentMap.nodes;
        float maxY = 0f;
        foreach (var node in nodes)
            maxY = Mathf.Max(maxY, node.gridPosition.y * offsetY);

        RectTransform contentRect = contentContainer.GetComponent<RectTransform>();
        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxY + 120f);
    }
}