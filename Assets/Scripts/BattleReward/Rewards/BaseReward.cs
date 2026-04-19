using UnityEngine;

public abstract class BaseReward
{
    protected RewardButton button;

    public void setButton(RewardButton button) { this.button = button; }

    public abstract void collect();
    public abstract string getDescription();
    public abstract Sprite getIcon();
}