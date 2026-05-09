using UnityEngine;

[System.Serializable]
public class CooldownCondition : MoveCondition
{
    public int cooldown = 2;

    public override bool canUse(MoveState state, UnitMove move)
    {
        return state.getCooldown(this) <= 0;
    }

    public override void onUse(MoveState state, UnitMove move)
    {
        state.setCooldown(this, cooldown);
    }

    public override void onTurnEnd(MoveState state)
    {
        state.decrementCooldown(this);
    }
}