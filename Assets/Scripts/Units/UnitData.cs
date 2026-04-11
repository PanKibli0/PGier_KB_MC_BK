using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Unit/UnitData")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public GameObject graphicPrefab;
    public UnitAIType aiType;
    public int maxHealth;
    public List<UnitMove> moves;
    [SerializeReference] public List<BaseStatusEffect> startEffects;
}

public enum UnitAIType
{
    None,
    Normal,
    Elite,
    Boss
}