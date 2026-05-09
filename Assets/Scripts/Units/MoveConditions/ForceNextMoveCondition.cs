using UnityEngine;

[System.Serializable]
public class ForceNextMoveCondition : MoveCondition
{
    public UnitMove nextMove;

    public override bool canUse(MoveState state, UnitMove move)
    {
        return true;
    }
}