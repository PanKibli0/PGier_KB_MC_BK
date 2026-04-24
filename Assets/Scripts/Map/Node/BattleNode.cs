using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleDifficulty
{
    Normal,
    Elite,
    Boss
}

public class BattleNode : BaseNode
{
    public UnitData[] enemies;
    public BattleDifficulty difficulty;


    public override void execute()
    {
        GameManager.Instance.currentMapNode = this;
        GameManager.Instance.pendingBattleEnemies = enemies;
        GameManager.Instance.pendingBattleDifficulty = difficulty;
        SceneManager.LoadScene("BattleScene");
    }


    public override string getIconPath()
    {
        switch (difficulty)
        {
            case BattleDifficulty.Boss: return "Icons/battle_boss";
            case BattleDifficulty.Elite: return "Icons/battle_elite";
            default: return "Icons/battle_normal";
        }
    }
}