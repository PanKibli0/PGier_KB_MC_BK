using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    [Header("Floor Labels")]
    [SerializeField] private Transform floorLabelContainer;
    [SerializeField] private GameObject floorLabelPrefab;

    [Header("Scroll")]
    [SerializeField] private ScrollRect scrollRect;

    private GameManager gameManager;
    private float mapCenterOffsetX;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
        displayMap();
    }

    private void displayMap()
    {
        List<BaseNode> nodes = gameManager.currentMap.nodes;
        int currentFloor = gameManager.currentMap.currentFloor;

        float contentWidth = contentContainer.GetComponent<RectTransform>().rect.width;
        int columns = 5;
        float totalMapWidth = (columns - 1) * offsetX + nodeWidth;
        mapCenterOffsetX = (contentWidth - totalMapWidth) / 2f;

        int maxFloor = nodes[nodes.Count - 1].gridPosition.y;

        foreach (var node in nodes)
        {
            GameObject btnObj = Instantiate(nodeButtonPrefab, contentContainer);
            RectTransform rect = btnObj.GetComponent<RectTransform>();
            rect.anchoredPosition = getNodePosition(node);

            NodeButton nodeButton = btnObj.GetComponent<NodeButton>();
            nodeButton.init(node, currentFloor);

            if (node.gridPosition.y == maxFloor)
                rect.localScale = new Vector3(1.75f, 1.75f, 1f);
        }

        adjustContentSize();
        drawLines();
        spawnFloorLabels();
        scrollToCurrentFloor();
    }

    private Vector2 getNodePosition(BaseNode node)
    {
        return new Vector2(
            mapCenterOffsetX + node.gridPosition.x * offsetX,
            node.gridPosition.y * offsetY + 20f
        );
    }

    private void adjustContentSize()
    {
        List<BaseNode> nodes = gameManager.currentMap.nodes;
        float maxY = 0f;
        foreach (var node in nodes)
            maxY = Mathf.Max(maxY, node.gridPosition.y * offsetY);

        RectTransform contentRect = contentContainer.GetComponent<RectTransform>();
        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxY + 120f * 1.75f);

        if (floorLabelContainer != null)
        {
            RectTransform labelRect = floorLabelContainer.GetComponent<RectTransform>();
            labelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxY + 120f * 1.75f);
        }
    }

    private void spawnFloorLabels()
    {
        if (floorLabelContainer == null || floorLabelPrefab == null) return;

        List<BaseNode> nodes = gameManager.currentMap.nodes;
        HashSet<int> spawnedFloors = new HashSet<int>();

        foreach (var node in nodes)
        {
            int floor = node.gridPosition.y;
            if (!spawnedFloors.Add(floor)) continue;

            GameObject labelObj = Instantiate(floorLabelPrefab, floorLabelContainer);
            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchoredPosition = new Vector2(0f, floor * offsetY + 20f + nodeHeight / 2f);

            TMP_Text labelText = labelObj.GetComponentInChildren<TMP_Text>();
            if (labelText != null)
                labelText.text = $"P{floor + 1}";
        }
    }

    private void drawLines()
    {
        foreach (var node in gameManager.currentMap.nodes)
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

                Color lineColor = (node.isVisited && connection.isVisited) ? Color.white : Color.gray;
                lineDrawer.drawLine(start, end, lineColor);
            }
        }
    }

    private void scrollToCurrentFloor()
    {
        float contentHeight = contentContainer.GetComponent<RectTransform>().rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float currentFloorY = gameManager.currentMap.currentFloor * offsetY + 20f;

        float targetY = currentFloorY - viewportHeight / 2f;
        targetY = Mathf.Clamp(targetY, 0f, contentHeight - viewportHeight);

        scrollRect.verticalNormalizedPosition = targetY / (contentHeight - viewportHeight);
    }
}