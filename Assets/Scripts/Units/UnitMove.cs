using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewUnitMove", menuName = "Unit/UnitMove")]
public class UnitMove : ScriptableObject
{
    public string moveName;
    [SerializeReference] public List<BaseAction> actions;
    [SerializeReference] public MoveCondition condition;
    public float weight = 1f;
}