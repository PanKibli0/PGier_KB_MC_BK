using UnityEngine;

[System.Serializable]
public abstract class BaseStatusEffect
{
    [HideInInspector] public string effectName;
    [HideInInspector] public string iconPath;

    [HideInInspector] public bool isMergeable = true;
    [HideInInspector] public bool isDebuff = false;

    public virtual void onTurnStart(Unit owner) { }
    public virtual void onTurnEnd(Unit owner) { }
    public virtual void onDealDamage(Unit owner, Unit target, ref int damage) { }
    public virtual void onReceiveDamage(Unit owner, Unit source, ref int damage) { }
    public virtual void onApply(Unit owner) { }
    public virtual void onRemove(Unit owner) { }
    public virtual bool merge(BaseStatusEffect other) { return false; }
    
    public virtual string getDescription() { return ""; }
    public virtual string getMainText() { return ""; }
    public virtual string getSecondaryText() { return ""; }

    public BaseStatusEffect Clone()
    {
        return (BaseStatusEffect)this.MemberwiseClone();
    }
}