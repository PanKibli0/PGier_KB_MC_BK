using UnityEngine;

[System.Serializable]
public class HealAction : BaseAction
{
    public int healAmount;

    public override void execute(Unit target)
    {
        Debug.Log($"HealAction -> {healAmount}");
    }
}