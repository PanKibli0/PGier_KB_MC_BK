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

    public override string getCardDescription(Unit source = null, Unit target = null, bool applyEffects = false)
    {
        return $"Przywołaj <sprite name=\"SummonAction\"> {unitData.unitName}";
    }

    public override string getIconPath() { return "Icons/summonAction"; }
}