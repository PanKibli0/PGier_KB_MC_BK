using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicPool", menuName = "Relic/RelicPool")]
public class RelicPool : ScriptableObject
{
    public List<RelicData> relics = new();

    public RelicData GetRandomRelic()
    {
        if (relics == null || relics.Count == 0)
            return null;

        return relics[Random.Range(0, relics.Count)];
    }
}