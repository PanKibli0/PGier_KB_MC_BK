using UnityEngine;

[System.Serializable]
public class EnergyOnDamageEffect : BaseStatusEffect
{
    public int damageThreshold;
    public int energyPerTrigger;
    private int damageAccumulated;

    public EnergyOnDamageEffect()
    {
        effectName = "Gwiazda";
        isMergeable = true;
        isDebuff = false;
    }

    public override void onDealDamage(Unit owner, Unit target, ref int damage)
    {
        damageAccumulated += damage;
        while (damageAccumulated >= damageThreshold)
        {
            damageAccumulated -= damageThreshold;
            ActionEventBus.requestEnergyChange(energyPerTrigger);
        }
    }

    public override bool merge(BaseStatusEffect other)
    {
        EnergyOnDamageEffect o = (EnergyOnDamageEffect)other;
        damageThreshold = Mathf.Min(damageThreshold, o.damageThreshold);
        energyPerTrigger += o.energyPerTrigger;
        return false;
    }

    public override string getMainText() { return damageThreshold.ToString(); }
    public override string getSecondaryText() { return "<sprite name=\"atak\">"; }
    public override string getIconPath() { return GameManager.Instance?.selectedCharacter?.energySpriteName ?? ""; }

    public override string getDescription()
    {
        string sprite = GameManager.Instance?.selectedCharacter?.energySpriteName ?? "";
        return $"Za każde {damageThreshold} zadanych <sprite name=\"atak\"> obrażeń zyskaj {energyPerTrigger} <sprite name=\"{sprite}\"> energii.";
    }

    public override string getActionDescription()
    {
        return $"Nałóż Gwiazdę ({damageThreshold}) <sprite name=\"atak\">";
    }
}