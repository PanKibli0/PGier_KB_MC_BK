using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public EnemyType enemyType;
    public int maxHealth;

    public List<EnemyMove> moves;

    [SerializeReference] 
    public List<BaseStatusEffect> startEffects;
}

public enum EnemyType
{
    Normal,
    Elite,
    Boss
}