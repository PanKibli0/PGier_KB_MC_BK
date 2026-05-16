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
        discardPile.Clear();
        exhaustPile.Clear();

        OnDrawPileChanged?.Invoke(drawPile.Count);
        OnDiscardPileChanged?.Invoke(discardPile.Count);
        OnExhaustPileChanged?.Invoke(exhaustPile.Count);

        if (GameManager.Instance != null && GameManager.Instance.currentDeck != null)
            foreach (var cardData in GameManager.Instance.currentDeck)
                drawPile.Add(new Card(cardData));

        // DEBUG
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

        // Karty P - Power (efekty)
        for (int i = 1; i <= 5; i++)
        {
            CardData data = ScriptableObject.CreateInstance<CardData>();
            data.cost = Random.Range(1, 3);
            data.type = CardType.Power;
            data.cardName = $"Card P{i}/{data.cost}";

            AddEffectAction action = new AddEffectAction();
            action.targetType = TargetType.Self;

            if (i == 1)
            {
                PoisonEffect effect = new PoisonEffect();
                effect.stacks = 10;
                action.effectToAdd = effect;
            }
            else if (i == 2)
            {
                BleedingEffect effect = new BleedingEffect();
                effect.stacks = 20;
                action.effectToAdd = effect;
            }
            else if (i == 3)
            {
                StrengthEffect effect = new StrengthEffect();
                effect.value = 30;
                action.effectToAdd = effect;
            }
            else if (i == 4)
            {
                ThornsEffect effect = new ThornsEffect();
                effect.value = 40;
                action.effectToAdd = effect;
            }
            else if (i == 5)
            {
                RegenerationEffect effect = new RegenerationEffect();
                effect.stacks = 50;
                action.effectToAdd = effect;
            }

            data.actions = new List<BaseAction> { action };

            drawPile.Add(new Card(data));
        }

        // DEBUG: karta Power przywołująca sojusznika
        UnitData allyData = ScriptableObject.CreateInstance<UnitData>();
        allyData.graphicPrefab = Resources.Load<GameObject>("Characters/Player/Warrior/war");
        allyData.unitName = "TestAlly";
        allyData.maxHealth = 20;
        allyData.aiType = UnitAIType.None;
        allyData.moves = new List<UnitMove>();
        allyData.startEffects = new List<BaseStatusEffect>();

        // Ruch 1: Uleczenie 10 HP
        UnitMove healMove = ScriptableObject.CreateInstance<UnitMove>();
        healMove.moveName = "Heal";
        HealAction healAction = new HealAction();
        healAction.healAmount = 10;
        healAction.targetType = TargetType.Self;
        healMove.actions = new List<BaseAction> { healAction };
        allyData.moves.Add(healMove);

        // Ruch 2: Tarcza 5
        UnitMove blockMove5 = ScriptableObject.CreateInstance<UnitMove>();
        blockMove5.moveName = "Block 5";
        BlockAction block5 = new BlockAction();
        block5.blockAmount = 5;
        block5.targetType = TargetType.Self;
        blockMove5.actions = new List<BaseAction> { block5 };
        allyData.moves.Add(blockMove5);

        // Ruch 3: Tarcza 10
        UnitMove blockMove10 = ScriptableObject.CreateInstance<UnitMove>();
        blockMove10.moveName = "Block 10";
        BlockAction block10 = new BlockAction();
        block10.blockAmount = 10;
        block10.targetType = TargetType.Self;
        blockMove10.actions = new List<BaseAction> { block10 };
        allyData.moves.Add(blockMove10);

        // Ruch 4: Tarcza 15
        UnitMove blockMove15 = ScriptableObject.CreateInstance<UnitMove>();
        blockMove15.moveName = "Block 15";
        BlockAction block15 = new BlockAction();
        block15.blockAmount = 15;
        block15.targetType = TargetType.Self;
        blockMove15.actions = new List<BaseAction> { block15 };
        allyData.moves.Add(blockMove15);

        // Karta Summon
        CardData summonCardData = ScriptableObject.CreateInstance<CardData>();
        summonCardData.cardName = "Summon TestAlly";
        summonCardData.cost = 1;
        summonCardData.type = CardType.Power;

        SummonAction summonAction = new SummonAction();
        summonAction.targetType = TargetType.Self;
        summonAction.unitData = allyData;

        summonCardData.actions = new List<BaseAction> { summonAction };

        drawPile.Add(new Card(summonCardData));
        // END DEBUG

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
        Debug.Log("EXHAUST ADD: " + card.data.cardName +
             " | total: " + exhaustPile.Count);
        exhaustPile.Add(card);
        OnExhaustPileChanged?.Invoke(exhaustPile.Count);
    }

}
