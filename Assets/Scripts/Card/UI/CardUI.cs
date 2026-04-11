using UnityEngine;
using UnityEngine.UI;
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

    [Header("UI")]
    [SerializeField] private Image frontImage;
    [SerializeField] private Image cardArtImage;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private Image energyIcon;
    [SerializeField] private TMP_Text costText;

    [SerializeField] private Sprite[] frontSprites;

    private Unit currentHoverTarget;
    private bool canDrag = true;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Start()
    {
        if (EnergySystem.Instance != null)
            EnergySystem.Instance.OnEnergyChanged += updateCostColor;

        UnitsManager.Instance.player.OnEffectsChanged += onEffectsChanged;
    }

    void OnDestroy()
    {
        if (EnergySystem.Instance != null)
            EnergySystem.Instance.OnEnergyChanged -= updateCostColor;

        if (UnitsManager.Instance?.player != null)
            UnitsManager.Instance.player.OnEffectsChanged -= onEffectsChanged;
    }

    private void onEffectsChanged() { updateDescription();}

    public void updateDescription(Unit target = null)
    {
        if (card == null || card.actions == null) return;

        string description = "";
        foreach (var action in card.actions)
        {
            description += action.getCardDescription(UnitsManager.Instance.player, target) + "\n";
        }
        descText.text = description;
    }


    public void init(Card card)
    {
        this.card = card;

        if (card.data.image != null)
            cardArtImage.sprite = card.data.image;

        int typeIndex = (int)card.data.type;
        if (frontSprites != null && typeIndex < frontSprites.Length)
            frontImage.sprite = frontSprites[typeIndex];

        // energyIcon.sprite = ;

        nameText.text = card.data.cardName;
        descText.text = $"{card.data.cost} Energy\n{string.Join("\n", card.data.actions)}";
        costText.text = $"{card.currentCost}";


        handArea = GetComponentInParent<HandAreaUI>();

        updateCostColor();
        updateDescription();
    }

    private void updateCostColor()
    {
        if (costText == null) return;

        if (EnergySystem.Instance != null && EnergySystem.Instance.canAfford(card.currentCost))
            costText.color = Color.white;
        else
            costText.color = Color.red;
    }

    #region Drag And Drop System

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (EnergySystem.Instance != null && !EnergySystem.Instance.canAfford(card.currentCost))
        {
            canDrag = false;
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

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        Unit newTarget = null;
        foreach (var result in results)
        {
            newTarget = result.gameObject.GetComponent<Unit>();
            if (newTarget != null) break;
        }

        if (currentHoverTarget != newTarget)
        {
            currentHoverTarget = newTarget;
            updateDescription(currentHoverTarget);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            canDrag = true;
            return;
        }

        bool canPlay;

        currentHoverTarget = null;
        updateDescription(null);

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
                return true;
            }
        }

        selectedTarget = null;
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
        Debug.Log($"Zagrano kartę: {card.data.cardName}, ilość akcji: {card.actions.Count}");

        if (EnergySystem.Instance != null)
        
            EnergySystem.Instance.spendEnergy(card.currentCost);
        
        foreach (var action in card.actions)
        {
            switch (action.targetType)
            {
                case TargetType.Self:
                    action.execute(UnitsManager.Instance.player, UnitsManager.Instance.player);
                    break;

                case TargetType.SelectedEnemy:
                    if (selectedTarget != null && selectedTarget.unitType == UnitType.Enemy)
                        action.execute(selectedTarget, UnitsManager.Instance.player);
                    break;

                case TargetType.AllEnemies:
                    foreach (var enemy in UnitsManager.Instance.enemies)
                        action.execute(enemy, UnitsManager.Instance.player);
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