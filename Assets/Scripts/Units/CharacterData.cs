using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/CharacterData")]
public class CharacterData : BaseUnitData
{
    public List<CardData> startCards;
    public CardPool cardPool;
    public int startGold;

}