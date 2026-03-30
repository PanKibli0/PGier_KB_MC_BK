using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class CardPileSystem : MonoBehaviour
{
    public static CardPileSystem Instance;

    public List<Card> drawPile = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public List<Card> exhaustPile = new List<Card>();

    [SerializeField] private CardUIManager cardUIManager;

    public event Action<Card> OnCardDrawn;
    public event Action<int> OnDrawPileChanged;
    public event Action<int> OnDiscardPileChanged;
    public event Action<int> OnExhaustPileChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // DEBUG
    void Start()
    {
        drawPile.Clear();

        // Karty S - SelectedEnemy
        for (int i = 1; i <= 5; i++)
        {
            CardData data = ScriptableObject.CreateInstance<CardData>();
            data.cost = Random.Range(1, 5);
            data.cardName = $"Card S{i}/{data.cost}";
            
            DamageAction action = new DamageAction();
            action.targetType = TargetType.SelectedEnemy;
            action.damageAmount = 6;

            data.actions = new List<BaseAction> { action };

            drawPile.Add(new Card(data));
        }

        // Karty AS - Special
        for (int i = 1; i <= 5; i++)
        {
            CardData data = ScriptableObject.CreateInstance<CardData>();
            data.cost = Random.Range(1, 5);
            data.type = CardType.Skill;
            data.cardName = $"Card SA{i}/{data.cost}";

            BlockAction action = new BlockAction();
            action.targetType = TargetType.AllEnemies;
            action.blockAmount = 2;
            HealAction action2 = new HealAction();
            action2.targetType = TargetType.AllEnemies;
            action2.healAmount = 2;

            data.actions = new List<BaseAction> { action, action2};

            drawPile.Add(new Card(data));
        }

        // Karty A - AllEnemies
        for (int i = 1; i <= 5; i++)
        {
            CardData data = ScriptableObject.CreateInstance<CardData>();
            data.cost = Random.Range(1, 5);
            data.cardName = $"Card A{i}/{data.cost}";

            DamageAction action = new DamageAction();
            action.targetType = TargetType.AllEnemies;
            action.damageAmount = 4;

            data.actions = new List<BaseAction> { action };

            drawPile.Add(new Card(data));
        }

        shuffle(drawPile);

        OnDiscardPileChanged?.Invoke(discardPile.Count);
        OnDrawPileChanged?.Invoke(drawPile.Count);

        int end = Random.Range(3, 6);
        for (int i = 0; i < end; i++) drawCard();

    }
    // END DEBUG

    void refillDrawPile()
    {
        drawPile = new List<Card>(discardPile);
        discardPile.Clear();

        shuffle(drawPile);

        OnDiscardPileChanged?.Invoke(discardPile.Count);
    }

    void shuffle(List<Card> pile)
    {
        for (int i = 0; i < pile.Count; i++)
        {
            int randomIndex = Random.Range(i, pile.Count);
            (pile[i], pile[randomIndex]) = (pile[randomIndex], pile[i]);
        }
    }

    public void drawCard()
    {
        if (drawPile.Count == 0 && discardPile.Count == 0) return;

        if (drawPile.Count == 0)
            refillDrawPile();

        Card drawnCard = drawPile[0];
        drawPile.RemoveAt(0);

        OnCardDrawn?.Invoke(drawnCard);
        OnDrawPileChanged?.Invoke(drawPile.Count);

    }



    public void discardCard(Card card)
    {
        if (card == null) return;
        discardPile.Add(card);
        OnDiscardPileChanged?.Invoke(discardPile.Count);
    }

    public void exhaustCard(Card card)
    {
        if (card == null) return;
        exhaustPile.Add(card);
        OnExhaustPileChanged?.Invoke(exhaustPile.Count);
    }

}
