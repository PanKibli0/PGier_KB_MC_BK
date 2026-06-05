using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/CharacterData")]
public class CharacterData : BaseUnitData
{
    public List<StartCardEntry> startCards;
    public CardPool cardPool;
    public int startGold;
    public int baseDrawCount = 5;
    public int baseMaxEnergy = 3;
}

[System.Serializable]
public class StartCardEntry
{
    public CardData data;
    public int amount;
}