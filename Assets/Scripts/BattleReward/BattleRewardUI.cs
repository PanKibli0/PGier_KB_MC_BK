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
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        itemRewardPanel.init(tooltip, gameManager.playerInventory);

        BattleDifficulty difficulty = gameManager != null
            ? gameManager.pendingBattleDifficulty
            : BattleDifficulty.Normal;

        RewardGenerator rewardGenerator = new RewardGenerator(
            cardRewardPanel, itemRewardPanel, relicRewardPanel, rewardsList, itemDatabase);

        setRewards(rewardGenerator.generate(difficulty));
    }

    public void setRewards(List<BaseReward> rewards)
    {
        rewardsLeft = rewards.Count;

        foreach (var reward in rewards)
        {
            GameObject buttonObj = Instantiate(rewardButtonPrefab, rewardsContainer);
            RewardButton rewardButton = buttonObj.GetComponent<RewardButton>();
            rewardButton.init(reward);
            rewardButton.OnRewardCollected += onRewardCollected;
            rewardButtons.Add(buttonObj);
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
        gameManager.currentMapNode.onComplete();
        SceneManager.LoadScene("MapScene");
    }
}