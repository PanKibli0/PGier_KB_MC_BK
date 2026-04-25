using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [Header("Nodes")]
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject nodeButtonPrefab;

    [SerializeField] private float offsetX = 200f;
    [SerializeField] private float offsetY = 200f;

    [Header("Lines")]
    [SerializeField] private LineDrawer lineDrawer;

    [SerializeField] private float nodeWidth = 80f;
    [SerializeField] private float nodeHeight = 80f;
    [SerializeField] private float lineOffset = 15f;

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
        drawLines();
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

    private void drawLines()
    {
        foreach (var node in GameManager.Instance.currentMap.nodes)
        {
            Vector2 startPos = getNodePosition(node);
            Vector2 startCenter = startPos + new Vector2(nodeWidth / 2, nodeHeight / 2);

            foreach (var connection in node.connections)
            {
                Vector2 endPos = getNodePosition(connection);
                Vector2 endCenter = endPos + new Vector2(nodeWidth / 2, nodeHeight / 2);

                Vector2 dir = (endCenter - startCenter).normalized;
                Vector2 start = startCenter + dir * lineOffset;
                Vector2 end = endCenter - dir * lineOffset;

                Color lineColor = (connection.isVisited) ? Color.white : Color.gray;
                lineDrawer.drawLine(start, end, lineColor);
            }
        }
    }
}