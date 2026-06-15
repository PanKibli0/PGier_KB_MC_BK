using UnityEngine;

[System.Serializable]
public class HealthLostStrengthEffect : BaseStatusEffect
{
    public int strengthPerHit;

    public HealthLostStrengthEffect()
    {
        effectName = "Po trupach do celu";
        isMergeable = true;
        isDebuff = false;
    }

    public override void onHealthLost(Unit owner, int damageTaken)
    {
        StrengthEffect effect = new StrengthEffect();
        effect.value = strengthPerHit;
        owner.addEffect(effect);
    }

    public override bool merge(BaseStatusEffect other)
    {
        strengthPerHit += ((HealthLostStrengthEffect)other).strengthPerHit;
        return false;
    }

    public override string getMainText() { return strengthPerHit.ToString(); }
    public override string getSecondaryText() { return "<color=#FF4444>!</color>"; }
    public override string getIconPath() { return "Icons/sila"; }

    public override string getDescription()
    {
        return $"Za każdym razem gdy stracisz zdrowie zyskaj {strengthPerHit} <sprite name=\"sila\"> Siły.";
    }

    public override string getActionDescription()
    {
        return $"Nałóż Po trupach do celu ({strengthPerHit}) <sprite name=\"sila\">";
    }
}