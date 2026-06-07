using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleRewardUI : MonoBehaviour
{
    [SerializeField] private Transform rewardsContainer;
    [SerializeField] private GameObject rewardButtonPrefab;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text continueButtonText;
    [SerializeField] private CardRewardPanel cardRewardPanel;
    [SerializeField] private ItemRewardPanel itemRewardPanel;
    [SerializeField] private RelicRewardPanel relicRewardPanel;
    [SerializeField] private GameObject rewardsList;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private Tooltip tooltip;

    private List<GameObject> rewardButtons = new List<GameObject>();
    private int rewardsLeft;

    void Start()
    {
        itemRewardPanel.init(tooltip, GameManager.Instance.playerInventory);

        // DEBUG
        createDebugRewards();
        // END DEBUG
    }

    public void setRewards(List<BaseReward> rewards)
    {
        rewardsLeft = rewards.Count;

        foreach (var reward in rewards)
        {
            GameObject btnObj = Instantiate(rewardButtonPrefab, rewardsContainer);
            RewardButton btn = btnObj.GetComponent<RewardButton>();
            btn.init(reward);
            btn.OnRewardCollected += onRewardCollected;
            rewardButtons.Add(btnObj);
        }
    }

    private void onRewardCollected()
    {
        rewardsLeft--;
        if (rewardsLeft == 0)
            continueButtonText.text = "KONTYNUUJ";
    }

    public void onContinueButtonClick()
    {
        GameManager.Instance.currentMapNode.onComplete();
        SceneManager.LoadScene("MapScene");
    }

    private int getRandomGold() { return Random.Range(50, 100); }

    private List<CardData> getRandomCards()
    {
        List<CardData> cards = new List<CardData>();
        CardPool pool = GameManager.Instance.selectedCharacter.cardPool;
        if (pool == null || pool.cards.Count == 0) return cards;
        for (int i = 0; i < 3; i++)
            cards.Add(pool.cards[Random.Range(0, pool.cards.Count)]);
        return cards;
    }

    private List<ItemData> getRandomItems()
    {
        List<ItemData> result = new List<ItemData>();
        if (itemDatabase == null || itemDatabase.items == null) return result;

        List<ItemData> pool = new List<ItemData>(itemDatabase.items);
        for (int i = 0; i < 3 && pool.Count > 0; i++)
        {
            int idx = Random.Range(0, pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx);
        }
        return result;
    }

    private List<RelicData> getRandomRelics()
    {
        List<RelicData> pool = new List<RelicData>(GameManager.Instance.relicPool);

        List<RelicData> result = new List<RelicData>();
        int count = Mathf.Min(3, pool.Count);
        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx);
        }
        return result;
    }

    // DEBUG
    private void createDebugRewards()
    {
        GoldReward gold = new GoldReward();
        gold.amount = getRandomGold();

        CardReward card = new CardReward();
        card.cards = getRandomCards();
        card.panel = cardRewardPanel;
        card.rewardsList = rewardsList;

        ItemReward item = new ItemReward();
        item.items = getRandomItems();
        item.panel = itemRewardPanel;
        item.rewardsList = rewardsList;

        RelicReward relic = new RelicReward();
        relic.relics = getRandomRelics();
        relic.panel = relicRewardPanel;
        relic.rewardsList = rewardsList;

        setRewards(new List<BaseReward> { gold, card, item, relic });
    }
    // END DEBUG
}
