using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject nodeButtonPrefab;

    private float offsetX = 220f;
    private float offsetY = 400f;


    void Start()
    {
        displayMap();
    }


    private void displayMap()
    {
        List<BaseNode> nodes = GameManager.Instance.currentMap.nodes;

        foreach (var node in nodes)
        {
            GameObject btnObj = Instantiate(nodeButtonPrefab, contentContainer);
            RectTransform rect = btnObj.GetComponent<RectTransform>();
            Vector2 pos = getNodePosition(node);
            rect.anchoredPosition = pos;

            Debug.Log($"Node {node.GetType().Name} at grid ({node.gridPosition.x}, {node.gridPosition.y}) -> position ({pos.x}, {pos.y})");

            NodeButton btn = btnObj.GetComponent<NodeButton>();
            btn.init(node);
        }

        Debug.Log($"Content size: {contentContainer.GetComponent<RectTransform>().rect}");
    }


    private Vector2 getNodePosition(BaseNode node)
    {
        float x = node.gridPosition.y * offsetX;
        float y = node.gridPosition.x * offsetY;
        return new Vector2(x, y);
    }
}