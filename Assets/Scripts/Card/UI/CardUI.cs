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

    private bool canDrag = true;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }


    public void init(Card card)
    {
        this.card = card;

        // switch (card.data.type): case CardType.Attack: BACKGROUND IMAGE = attack card sprite; ITD
        // 

        nameText.text = card.data.cardName;
        handArea = GetComponentInParent<HandAreaUI>();
    }

    #region Drag And Drop System

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (EnergySystem.Instance != null && !EnergySystem.Instance.canAfford(card.currentCost))
        {
            canDrag = false;
            Debug.Log($"Nie stać na kartę {card.data.cardName} (koszt: {card.currentCost})");

            return;
        }

        canDrag = true;
        startParent = transform.parent;
        startSiblingIndex = transform.GetSiblingIndex();

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            canDrag = true;
            return;
        }

        bool canPlay;

        if (cardRequiresTarget())
            canPlay = canPlayWithTarget(eventData);
        else
            canPlay = isOverPlayArea(eventData);

        if (canPlay)
            playCard();
        else
            returnToHand();
    }
    #endregion


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

        if (EnergySystem.Instance != null)
        
            EnergySystem.Instance.spendEnergy(card.currentCost);
        

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