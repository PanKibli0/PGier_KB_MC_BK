using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Drag And Drop System")]
    private RectTransform rectTransform;
    private Canvas canvas;
    private Transform startParent;
    private int startSiblingIndex;
    private HandAreaUI handArea;
    private Unit selectedTarget;

    [Header("Card Data")]
    private Card card;
    public TMP_Text nameText;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }


    public void init(Card card)
    {
        this.card = card;

        nameText.text = card.data.cardName;
        handArea = GetComponentInParent<HandAreaUI>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        startSiblingIndex = transform.GetSiblingIndex();

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        transform.position = eventData.position;
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        bool canPlay;

        if (cardRequiresTarget())
            // FUTURE: System targetowania 
            canPlay = canPlayWithTarget(eventData);
        else
            // FUTURE: System zagrywania kart 
            canPlay = isOverPlayArea(eventData);

        if (canPlay)
            // FUTURE: System zagrywania kart 
            playCard();
        else
            returnToHand();
    }

    // FUTURE: IDK -> Card 
    private bool cardRequiresTarget()
    {
        if (card == null || card.actions == null) return false;

        foreach (var action in card.actions)
        {
            if (action.requiresTarget()) return true;
        }
        return false;
    }

    // FUTURE: System targetowania
    private bool canPlayWithTarget(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            Unit targetUnit = result.gameObject.GetComponent<Unit>();
            if (targetUnit != null)
            {
                selectedTarget = targetUnit;
                Debug.Log($"Trafiono w cel: {targetUnit.unitName}");
                return true;
            }
        }

        selectedTarget = null;
        Debug.Log("Nie trafiono w żaden cel");
        return false;
    }

    // FUTURE: System zagrywania kart
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
    
    // FUTURE: System zagrywania kart
    private void playCard()
    {
        Debug.Log($"Zagrano kartę: {card.data.cardName}");

        foreach (var action in card.actions)
        {
            switch (action.targetType)
            {
                case TargetType.SelectedEnemy:
                    if (selectedTarget != null && selectedTarget.unitType == UnitType.Enemy)
                        action.execute(selectedTarget);
                    break;

                case TargetType.AllEnemies:
                    // DEBUG
                    Unit[] allUnits = FindObjectsByType<Unit>(FindObjectsSortMode.None);
                    foreach (var unit in allUnits)
                        if (unit.unitType == UnitType.Enemy)
                            action.execute(unit);
                    // END DEBUG
                    break;
            }
        }

        HandSystem.Instance.removeCard(card);
        Destroy(gameObject);
    }


    private void returnToHand()
    {
        transform.SetParent(startParent);
        transform.SetSiblingIndex(startSiblingIndex);
        handArea?.RefreshLayout();
    }
}