using System.Collections.Generic;
using UnityEngine;

public class RewardGenerator
{
    private const int GOLD_NORMAL_MIN = 35;
    private const int GOLD_NORMAL_MAX = 150;
    private const int GOLD_NORMAL_STEP = 5;

    private const int GOLD_ELITE_MIN = 100;
    private const int GOLD_ELITE_MAX = 200;
    private const int GOLD_ELITE_STEP = 5;

    private const int GOLD_BOSS_MIN = 150;
    private const int GOLD_BOSS_MAX = 350;
    private const int GOLD_BOSS_STEP = 25;

    private const float ITEM_CHANCE_NORMAL = 0.30f;
    private const float ITEM_CHANCE_ELITE = 0.60f;

    private const float RELIC_CHANCE_ELITE = 0.30f;
    private const float RELIC_CHANCE_BOSS = 1f;

    private const float ELITE_BONUS_RARE = 15f;
    private const float ELITE_BONUS_LEGENDARY = 5f;

    private float[] rarityNormalZone1 = { 75f, 20f, 5f };
    private float[] rarityNormalZone2 = { 60f, 32.5f, 7.5f };
    private float[] rarityNormalZone3 = { 45f, 45f, 10f };
    private float[] rarityBoss = { 10f, 40f, 50f };

    private CardRewardPanel cardPanel;
    private ItemRewardPanel itemPanel;
    private RelicRewardPanel relicPanel;
    private GameObject rewardsList;
    private ItemDatabase itemDatabase;
    private GameManager gameManager;

    public RewardGenerator(CardRewardPanel cardPanel, ItemRewardPanel itemPanel,
        RelicRewardPanel relicPanel, GameObject rewardsList, ItemDatabase itemDatabase)
    {
        this.cardPanel = cardPanel;
        this.itemPanel = itemPanel;
        this.relicPanel = relicPanel;
        this.rewardsList = rewardsList;
        this.itemDatabase = itemDatabase;
        gameManager = GameManager.Instance;
    }

    public List<BaseReward> generate(BattleDifficulty difficulty)
    {
        List<BaseReward> rewards = new List<BaseReward>();
        int currentFloor = gameManager != null ? gameManager.currentMap.currentFloor : 1;

        GoldReward goldReward = new GoldReward();
        goldReward.amount = rollGold(difficulty);
        rewards.Add(goldReward);

        CardReward cardReward = new CardReward();
        cardReward.init(getRandomCards(difficulty, currentFloor, 3), cardPanel, rewardsList);
        rewards.Add(cardReward);

        if (Random.value < getItemChance(difficulty) && itemDatabase != null)
        {
            ItemReward itemReward = new ItemReward();
            itemReward.init(getRandomItems(3), itemPanel, rewardsList);
            rewards.Add(itemReward);
        }

        if (Random.value < getRelicChance(difficulty))
        {
            RelicReward relicReward = new RelicReward();
            relicReward.init(getRandomRelics(3), relicPanel, rewardsList);
            rewards.Add(relicReward);
        }

        return rewards;
    }

    private int rollGold(BattleDifficulty difficulty)
    {
        switch (difficulty)
        {
            case BattleDifficulty.Elite:
                {
                    int steps = Random.Range(0, (GOLD_ELITE_MAX - GOLD_ELITE_MIN) / GOLD_ELITE_STEP + 1);
                    return GOLD_ELITE_MIN + steps * GOLD_ELITE_STEP;
                }
            case BattleDifficulty.Boss:
                {
                    int steps = Random.Range(0, (GOLD_BOSS_MAX - GOLD_BOSS_MIN) / GOLD_BOSS_STEP + 1);
                    return GOLD_BOSS_MIN + steps * GOLD_BOSS_STEP;
                }
            default:
                {
                    int steps = Random.Range(0, (GOLD_NORMAL_MAX - GOLD_NORMAL_MIN) / GOLD_NORMAL_STEP + 1);
                    return GOLD_NORMAL_MIN + steps * GOLD_NORMAL_STEP;
                }
        }
    }

    private float getItemChance(BattleDifficulty difficulty)
    {
        switch (difficulty)
        {
            case BattleDifficulty.Elite: return ITEM_CHANCE_ELITE;
            case BattleDifficulty.Boss: return 0f;
            default: return ITEM_CHANCE_NORMAL;
        }
    }

    private float getRelicChance(BattleDifficulty difficulty)
    {
        switch (difficulty)
        {
            case BattleDifficulty.Elite: return RELIC_CHANCE_ELITE;
            case BattleDifficulty.Boss: return RELIC_CHANCE_BOSS;
            default: return 0f;
        }
    }

    private float[] getRarityWeights(BattleDifficulty difficulty, int floor)
    {
        if (difficulty == BattleDifficulty.Boss)
            return rarityBoss;

        float[] baseWeights;

        if (floor <= 4) baseWeights = rarityNormalZone1;
        else if (floor <= 8) baseWeights = rarityNormalZone2;
        else baseWeights = rarityNormalZone3;

        if (difficulty == BattleDifficulty.Elite)
        {
            float[] eliteWeights = new float[3];
            eliteWeights[0] = Mathf.Max(0f, baseWeights[0] - ELITE_BONUS_RARE - ELITE_BONUS_LEGENDARY);
            eliteWeights[1] = Mathf.Max(0f, baseWeights[1] + ELITE_BONUS_RARE);
            eliteWeights[2] = Mathf.Max(0f, baseWeights[2] + ELITE_BONUS_LEGENDARY);
            return eliteWeights;
        }

        return baseWeights;
    }

    private Rarity rollRarity(float[] weights)
    {
        float total = 0f;
        foreach (float w in weights) total += w;

        float roll = Random.Range(0f, total);
        float cumulative = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (roll <= cumulative)
                return (Rarity)i;
        }

        return Rarity.Common;
    }

    private List<CardData> getRandomCards(BattleDifficulty difficulty, int floor, int count)
    {
        List<CardData> result = new List<CardData>();

        CardPool cardPool = gameManager != null ? gameManager.selectedCharacter.cardPool : null;
        if (cardPool == null || cardPool.cards == null || cardPool.cards.Count == 0)
            return result;

        float[] weights = getRarityWeights(difficulty, floor);

        List<CardData> commons = new List<CardData>();
        List<CardData> rares = new List<CardData>();
        List<CardData> legendaries = new List<CardData>();

        foreach (CardData card in cardPool.cards)
        {
            switch (card.rarity)
            {
                case Rarity.Common: commons.Add(card); break;
                case Rarity.Rare: rares.Add(card); break;
                case Rarity.Legendary: legendaries.Add(card); break;
            }
        }

        HashSet<CardData> chosen = new HashSet<CardData>();
        int maxAttempts = count * 10;
        int attempts = 0;

        while (result.Count < count && attempts < maxAttempts)
        {
            attempts++;
            Rarity rarity = rollRarity(weights);
            List<CardData> source = getPoolForRarity(rarity, commons, rares, legendaries);

            if (source.Count == 0) continue;

            CardData pick = source[Random.Range(0, source.Count)];
            if (chosen.Add(pick))
                result.Add(pick);
        }

        return result;
    }

    private List<CardData> getPoolForRarity(Rarity rarity, List<CardData> commons, List<CardData> rares, List<CardData> legendaries)
    {
        switch (rarity)
        {
            case Rarity.Rare:
                return rares.Count > 0 ? rares : commons;
            case Rarity.Legendary:
                if (legendaries.Count > 0) return legendaries;
                if (rares.Count > 0) return rares;
                return commons;
            default:
                return commons.Count > 0 ? commons : rares;
        }
    }

    private List<ItemData> getRandomItems(int count)
    {
        List<ItemData> result = new List<ItemData>();
        if (itemDatabase == null || itemDatabase.items == null) return result;

        List<ItemData> available = new List<ItemData>(itemDatabase.items);
        for (int i = 0; i < count && available.Count > 0; i++)
        {
            int idx = Random.Range(0, available.Count);
            result.Add(available[idx]);
            available.RemoveAt(idx);
        }
        return result;
    }

    private List<RelicData> getRandomRelics(int count)
    {
        List<RelicData> result = new List<RelicData>();
        if (gameManager == null || gameManager.relicPool == null) return result;

        List<RelicData> available = new List<RelicData>(gameManager.relicPool.relics);
        for (int i = 0; i < count && available.Count > 0; i++)
        {
            int idx = Random.Range(0, available.Count);
            result.Add(available[idx]);
            available.RemoveAt(idx);
        }
        return result;
    }
}