using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyPool", menuName = "EnemyPool")]
public class EnemyPool : ScriptableObject
{
    public List<EnemyPreset> normalFights;
    public List<EnemyPreset> eliteFights;
    public List<EnemyPreset> bossFights;
}

[System.Serializable]
public class EnemyPreset
{
    public UnitData[] enemies;
}