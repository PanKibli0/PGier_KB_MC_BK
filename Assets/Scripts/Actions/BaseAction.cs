using UnityEngine;

[System.Serializable]
public abstract class BaseAction
{
    public TargetType targetType;

    public abstract void execute(Unit target);

    public BaseAction Clone() // GŁĘBOKIE KOPIOWANIE
    {
        return (BaseAction)this.MemberwiseClone();
    }

    public bool requiresTarget()
    {
        return targetType == TargetType.SelectedEnemy ||
               targetType == TargetType.SelectedAlly ||
               targetType == TargetType.SelectedAllyOrSelf;
    }
}



public enum TargetType
{
    Self,               // na siebie
    SelectedEnemy,      // wybrany wróg
    AllEnemies,         // wszyscy wrogowie
    RandomEnemy,        // losowy wróg
    SelectedAllyOrSelf,  // wybrany sojusznik lub ja
    SelectedAlly,       // wybrany sojusznik (bez siebie)
    AllAllies,          // wszyscy sojusznicy (bez siebie)
    AllAlliesAndSelf,   // wszyscy sojusznicy + ja
    RandomAlly,         // losowy sojusznik (bez siebie)
    RandomUnit,         // losowa jednostka (wróg lub sojusznik)
    AllUnits           // wszystkie jednostki  
}