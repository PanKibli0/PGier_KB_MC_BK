using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Unit/UnitData")]
public class UnitData : BaseUnitData
{
    public UnitAIType aiType;
    public List<UnitMove> moves;
}

public enum UnitAIType
{
    None,
    Normal,
    Elite,
    Boss
}