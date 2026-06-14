using UnityEngine;

[System.Serializable]
public class BombEffect : BaseStatusEffect
{
    public int damage;
    public int turns;

    public BombEffect()
    {
        effectName = "Bomba";
        isMergeable = false;
        isDebuff = true;
    }

    public override string getMainText() { return damage.ToString(); }
    public override string getSecondaryText() { return turns.ToString(); }
    public override string getIconPath() { return "Icons/Bomba"; }

    public override void onTurnEnd(Unit owner)
    {
        turns--;
        if (turns <= 0)
        {
            owner.takeDamage(damage, DamageType.Normal);
            owner.removeEffect(this);
        }
    }

    public override string getDescription()
    {
        if (turns == 1) return $"Na koniec tury zadaje {damage} <sprite name=\"atak\"> obrażeń.";

        return $"Po {turns} turach zada {damage} <sprite name=\"atak\"> obrażeń.";
    }

    public override string getActionDescription()
    {
        if (turns > 0)
            return $"Nałóż Bombę <sprite name=\"bomba\">";
        else
            return $"Usuń Bombę";
    }
}