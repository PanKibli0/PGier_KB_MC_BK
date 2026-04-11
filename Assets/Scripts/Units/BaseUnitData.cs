using UnityEngine;
using System.Collections.Generic;

public abstract class BaseUnitData : ScriptableObject
{
    public string unitName;
    public GameObject graphicPrefab;
    public int maxHealth;
    [SerializeReference] public List<BaseStatusEffect> startEffects;
}