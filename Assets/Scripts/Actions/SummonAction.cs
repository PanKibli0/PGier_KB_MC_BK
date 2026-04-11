using UnityEngine;

[System.Serializable]
public class SummonAction : BaseAction
{
    public UnitData unitData;

    public override void execute(Unit target, Unit source)
    {
        if (unitData == null) return;

        UnitType summonedType = (source.unitType == UnitType.Player || source.unitType == UnitType.Ally) ? UnitType.Ally : UnitType.Enemy;

        UnitsManager.Instance.spawn(unitData, summonedType);
    }
}