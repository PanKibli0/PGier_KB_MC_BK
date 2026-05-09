using UnityEngine;

[System.Serializable]
public class RequirePreviousMoveCondition : MoveCondition
{
    public UnitMove requiredMove;

    public override bool canUse(MoveState state, UnitMove move)
    {
        return state.lastUsedMove == requiredMove;
    }
}