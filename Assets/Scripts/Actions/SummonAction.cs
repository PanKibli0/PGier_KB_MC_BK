using UnityEngine;

[System.Serializable]
public class SummonAction : BaseAction
{
    public UnitData unitData;

    public override void execute(Unit target, Unit source)
    {
        if (unitData == null) return;

        UnitType summonedType;

        if (source.unitType == UnitType.Player || source.unitType == UnitType.Ally)
            summonedType = UnitType.Ally;
        else
            summonedType = UnitType.Enemy;

        UnitsManager.Instance.spawn(unitData, summonedType);
    }
    public override string getIconPath() { return "Icons/obrona"; }
}