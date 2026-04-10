using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewUnitMove", menuName = "Unit/UnitMove")]
public class UnitMove : ScriptableObject
{
    // TODO: waga move, aby niektóre były bardziej prawdopodobne niż inne
    // TODO: warunki do użycia move

    public string moveName;
    [SerializeReference] public List<BaseAction> actions;
}