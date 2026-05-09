using UnityEngine;

[System.Serializable]
public class TurnCondition : MoveCondition
{
    public int requiredTurn;

    public override bool canUse(MoveState state, UnitMove move)
    {
        return state.currentTurn == requiredTurn;
    }
}