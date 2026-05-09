using UnityEngine;

public enum HealthConditionType
{
    Below,
    Above,
    BelowPercent,
    AbovePercent
}

[System.Serializable]
public class HealthCondition : MoveCondition
{
    public HealthConditionType type;
    public int value;

    public override bool canUse(MoveState state, UnitMove move)
    {
        if (state.owner == null)
            return false;

        Unit unit = state.owner;

        if (type == HealthConditionType.BelowPercent)
        {
            float percent = (float)unit.currentHealth / unit.currentMaxHealth * 100f;
            return percent < value;
        }
        if (type == HealthConditionType.AbovePercent)
        {
            float percent = (float)unit.currentHealth / unit.currentMaxHealth * 100f;
            return percent > value;
        }
        if (type == HealthConditionType.Below)
            return unit.currentHealth < value;
        if (type == HealthConditionType.Above)
            return unit.currentHealth > value;

        return false;
    }
}