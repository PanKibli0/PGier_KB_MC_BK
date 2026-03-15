using UnityEngine;

[CreateAssetMenu(fileName = "CardActionData", menuName = "Cards/CardActionData")]
public abstract class CardActionData : ScriptableObject
{
    public abstract void Execute(int value); // TODO: target
}
