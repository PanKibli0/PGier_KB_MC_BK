using UnityEngine;

public class HealReward : BaseReward
{
    public int amount;

    public override void collect()
    {
        GameManager.Instance.currentHealth += amount;
        if (GameManager.Instance.currentHealth > GameManager.Instance.maxHealth)
            GameManager.Instance.currentHealth = GameManager.Instance.maxHealth;
        
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