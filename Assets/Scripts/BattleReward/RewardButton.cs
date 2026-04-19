using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RewardButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text descriptionText;

    private BaseReward reward;
    public event Action OnRewardCollected;

    public void init(BaseReward reward)
    {
        this.reward = reward;
        reward.setButton(this);
        //icon.sprite = reward.getIcon();
        descriptionText.text = reward.getDescription();
    }

    public void onClick()
    {
        reward.collect();
    }

    public void destroyButton()
    {
        OnRewardCollected?.Invoke();
        Destroy(gameObject);
    }
}