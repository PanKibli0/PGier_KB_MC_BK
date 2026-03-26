using UnityEngine;

[System.Serializable]
public class DamageAction : BaseAction
{
    public int damageAmount;

    public override void execute(Unit target)
    {
        //Debug.Log($"DamageAction -> {damageAmount}");
        target.takeDamage(damageAmount);
    }
}