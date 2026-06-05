using UnityEngine;

public class HealReward : BaseReward
{
    public int amount;

    public override void collect()
    {
        GameManager.Instance.setHealth(GameManager.Instance.currentHealth + amount);

        button.destroyButton();
    }

    public override string getDescription()
    {
        return $"Ulecz {amount} życia";
    }

    public override Sprite getIcon()
    {
        return null;
    }
}