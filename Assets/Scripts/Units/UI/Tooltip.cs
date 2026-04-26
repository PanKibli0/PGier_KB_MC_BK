using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Transform contentContainer;
    [SerializeField] private TooltipEntry entryPrefab;
    [SerializeField] private ScrollRect scrollRect;

    private RectTransform rectTransform;
    private float maxHeight = 600f;
    public static Tooltip Instance;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void OnDisable()
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
            EventSystem.current.SetSelectedGameObject(null);
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition += scroll * 0.5f;
        }
    }

    public void setPositionSide(bool isRight)
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        Vector2 currentPos = rectTransform.anchoredPosition;

        if (isRight)
            rectTransform.anchoredPosition = new Vector2(400f, currentPos.y);
        else
            rectTransform.anchoredPosition = new Vector2(-400f, currentPos.y);
    }


    public void show(List<(Sprite icon, string name, string description)> entries)
    {
        
        clearEntries();

        foreach (var entry in entries)
        {
            TooltipEntry tooltipEntry = Instantiate(entryPrefab, contentContainer);
            tooltipEntry.init(entry.icon, entry.name, entry.description);
        }

        gameObject.SetActive(true);
        refreshLayout();
    }


    private void refreshLayout()
    {
        float totalHeight = 0f;

        foreach (Transform child in contentContainer)
        {
            TooltipEntry entry = child.GetComponent<TooltipEntry>();
            if (entry != null)
                totalHeight += entry.getHeight();
        }

        RectTransform contentRect = contentContainer.GetComponent<RectTransform>();
        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);

        if (totalHeight > maxHeight)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 65f);
        }
        else
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0f);
        }
    }

    

    public void hide()
    {
        clearEntries();
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
    }

    private void clearEntries()
    {
        foreach (Transform child in contentContainer)
            Destroy(child.gameObject);
    }
}