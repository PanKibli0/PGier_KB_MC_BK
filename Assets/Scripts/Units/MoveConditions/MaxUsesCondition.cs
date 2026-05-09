using UnityEngine;

[System.Serializable]
public class MaxUsesCondition : MoveCondition
{
    public int maxUses = 3;

    public override bool canUse(MoveState state, UnitMove move)
    {
        return state.getUseCount(move) < maxUses;
    }
}