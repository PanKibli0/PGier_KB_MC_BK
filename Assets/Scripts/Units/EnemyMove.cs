using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnemyMove", menuName = "Enemy/EnemyMove")]
public class EnemyMove : ScriptableObject
{
    // TODO: waga move, aby niektóre były bardziej prawdopodobne niż inne
    // TODO: warunki do użycia move

    public string moveName;

    [SerializeReference]
    public List<BaseAction> actions;
}