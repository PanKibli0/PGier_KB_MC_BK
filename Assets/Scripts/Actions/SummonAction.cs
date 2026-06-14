using UnityEngine;

[System.Serializable]
public class SummonAction : BaseAction
{
    public UnitData unitData;

    public override void execute(Unit target, Unit source)
    {
        UnitType summonedType;

        if (source.unitType == UnitType.Player || source.unitType == UnitType.Ally)
            summonedType = UnitType.Ally;
        else
            summonedType = UnitType.Enemy;

        ActionEventBus.requestSummon(unitData, summonedType);
    }

    public override string getIconPath() { return "Icons/obrona"; }
}