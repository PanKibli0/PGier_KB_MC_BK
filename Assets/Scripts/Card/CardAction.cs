using UnityEngine;


[System.Serializable]
public class CardAction
{
    //public CardActionData actionData;
    public int value;
    public TargetType target;
}



public enum TargetType
{
    Self,
    SelectedEnemy,
    AllEnemies,
    RandomEnemy,
    SelectedAlly,
    AllAllies,
    AllAlliesAndSelf,
    RandomAlly,
    RandomUnit,
    AllUnits
}