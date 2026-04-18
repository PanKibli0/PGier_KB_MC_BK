using UnityEngine;

[System.Serializable]
public abstract class BaseAction
{
    public TargetType targetType;

    public abstract void execute(Unit target, Unit source);

    public BaseAction Clone() // GŁĘBOKIE KOPIOWANIE
    {
        return (BaseAction)this.MemberwiseClone();
    }

    public bool requiresTarget()
    {
        return targetType == TargetType.SelectedEnemy ||
               targetType == TargetType.SelectedAlly;
    }

    public virtual string getCardDescription(Unit source, Unit target = null) { return ""; }

    public virtual Sprite getIcon() { return null; }
    public virtual string getValue() { return ""; }
}



public enum TargetType
{
    Self,               // na siebie
    SelectedEnemy,      // wybrany wróg
    SelectedAlly,       // wybrany sojusznik lub ja
    RandomEnemy,        // losowy wróg
    RandomAlly,         // losowy sojusznik lub ja
    RandomUnit,         // losowa jednostka (wróg/sojusznik/ja)
    AllEnemies,         // wszyscy wrogowie
    AllAllies,          // wszyscy sojusznicy i ja
    AllUnits            // wszystkie jednostki
}