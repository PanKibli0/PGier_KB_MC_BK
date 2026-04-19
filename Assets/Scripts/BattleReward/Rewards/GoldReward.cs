using UnityEngine;

public class GoldReward : BaseReward
{
    public int amount;

    public override void collect()
    {
        GameManager.Instance.addGold(amount);
        button.destroyButton();
    }

    public override string getDescription()
    {
        return $"Zdobądź {amount} złota";
    }

    public override Sprite getIcon()
    {
        return null;
    }
}