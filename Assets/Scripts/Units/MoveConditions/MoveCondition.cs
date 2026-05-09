using UnityEngine;

[System.Serializable]
public abstract class MoveCondition
{
    public bool mandatory;

    public abstract bool canUse(MoveState state, UnitMove move);
    public virtual void onUse(MoveState state, UnitMove move) { }
    public virtual void onTurnEnd(MoveState state) { }
}