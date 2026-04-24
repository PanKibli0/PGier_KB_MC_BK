using UnityEngine;

public class BattleSetup : MonoBehaviour
{
    void Start()
    {
        CharacterData character = GameManager.Instance.selectedCharacter;
        if (character != null)
            UnitsManager.Instance.spawnPlayer(character);

        UnitData[] enemies = GameManager.Instance.pendingBattleEnemies;
        if (enemies != null)
        {
            foreach (var data in enemies)
            {
                if (data != null)
                    UnitsManager.Instance.spawn(data, UnitType.Enemy);
            }
        }

        GameManager.Instance.pendingBattleEnemies = null;
    }
}