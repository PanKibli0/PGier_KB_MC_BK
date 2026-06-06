using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private Image icon;

    private ItemData item;
    private ItemRewardPanel panel;
    private int index;
    private Tooltip tooltip;
    private PlayerInventory inventory;
    private Unit playerUnit;
    private bool positionTooltip;
    private bool requiresTarget;

    private Canvas canvas;
    private RectTransform canvasRect;
    private Transform originalParent;
    private int originalSiblingIndex;
    private bool isDragging;
    private Unit selectedTarget;

    public void setup(ItemData item, int index, Tooltip tooltip, PlayerInventory inventory, Unit playerUnit)
    {
        this.item = item;
        this.index = index;
        this.tooltip = tooltip;
        this.inventory = inventory;
        this.playerUnit = playerUnit;
        positionTooltip = true;
        requiresTarget = itemRequiresTarget();
        canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
            canvasRect = canvas.rootCanvas.GetComponent<RectTransform>();
        refreshUI();
    }

    public void setupReward(ItemData item, ItemRewardPanel panel, Tooltip tooltip)
    {
        this.item = item;
        this.panel = panel;
        this.tooltip = tooltip;
        positionTooltip = false;
        refreshUI();
    }

    private void refreshUI()
    {
        if (item == null) return;
        if (icon != null)
            icon.sprite = item.icon;
    }

    private bool itemRequiresTarget()
    {
        if (item == null || item.actions == null) return false;
        foreach (var action in item.actions)
            if (action.requiresTarget()) return true;
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging) return;

        if (panel != null)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                panel.selectItem(item);
        }
        else
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                tooltip?.hide();
                inventory?.removeItemAt(index);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (panel != null) return;
        if (playerUnit == null) return;
        if (canvas == null) return;

        isDragging = true;
        tooltip?.hide();

        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        transform.SetParent(canvas.rootCanvas.transform);
        transform.SetAsLastSibling();

        MoveToPointer(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        MoveToPointer(eventData);
    }

    private void MoveToPointer(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                eventData.position,
                null,
                out Vector2 localPoint))
        {
            ((RectTransform)transform).localPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;

        bool canUse = requiresTarget ? tryGetTarget(eventData) : isOverPlayArea(eventData);

        if (canUse)
        {
            inventory?.useItem(index, playerUnit, selectedTarget);
            Destroy(gameObject);
        }
        else
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
        }
    }

    private bool tryGetTarget(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            Unit target = result.gameObject.GetComponent<Unit>();
            if (target == null)
                target = result.gameObject.GetComponentInParent<Unit>();
            if (target != null)
            {
                selectedTarget = target;
                return true;
            }
        }

        selectedTarget = null;
        return false;
    }

    private bool isOverPlayArea(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("PlayArea")) return true;
        }
        return false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging || item == null || tooltip == null) return;

        var entries = new List<(Sprite, string, string)>
        {
            (item.icon, item.itemName, item.getDescription())
        };

        if (positionTooltip)
        {
            tooltip.show(entries);
            RectTransform rt = GetComponent<RectTransform>();
            Vector3 pos = rt.position;
            pos.x += rt.rect.width;
            pos.y -= rt.rect.height;
            tooltip.transform.position = pos;
        }
        else
        {
            Vector3 savedPos = tooltip.transform.position;
            tooltip.show(entries);
            tooltip.transform.position = savedPos;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
            tooltip?.hide();
    }
}