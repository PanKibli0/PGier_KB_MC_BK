using UnityEngine;

public class BattleSetup : MonoBehaviour
{
    [SerializeField] private UnitData[] enemiesData;

    void Start()
    {
        CharacterData character = GameManager.Instance.selectedCharacter;
        if (character != null)
        {
            UnitsManager.Instance.spawnPlayer(character);
        }

        foreach (var data in enemiesData)
        {
            UnitsManager.Instance.spawn(data, UnitType.Enemy);
        }
    }
}