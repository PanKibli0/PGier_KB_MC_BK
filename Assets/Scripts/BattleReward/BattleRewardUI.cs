using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BattleRewardUI : MonoBehaviour
{
    [SerializeField] private Transform rewardsContainer;
    [SerializeField] private GameObject rewardButtonPrefab;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text continueButtonText;
    [SerializeField] private CardRewardPanel cardRewardPanel;
    [SerializeField] private GameObject rewardsList;

    private List<GameObject> rewardButtons = new List<GameObject>();
    private int rewardsLeft;

    // DEBUG
    void Start()
    {
        createDebugRewards();
    }
    // END DEBUG

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
        {
            continueButtonText.text = "KONTYNUUJ";
        }
    }

    public void onContinueButtonClick()
    {
        GameManager.Instance.currentMapNode.onComplete();
        SceneManager.LoadScene("MapScene");
    }

    private int getRandomGold()
    {
        return Random.Range(50, 100);
    }

    private List<CardData> getRandomCards()
    {
        List<CardData> cards = new List<CardData>();
        CardPool pool = GameManager.Instance.selectedCharacter.cardPool;

        if (pool == null || pool.cards.Count == 0) return cards;

        for (int i = 0; i < 3; i++)
        {
            CardData card = pool.cards[Random.Range(0, pool.cards.Count)];
            cards.Add(card);
        }

        return cards;
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

        List<BaseReward> rewards = new List<BaseReward>();
        rewards.Add(gold);
        rewards.Add(card);

        setRewards(rewards);
    }
    // END DEBUG
}