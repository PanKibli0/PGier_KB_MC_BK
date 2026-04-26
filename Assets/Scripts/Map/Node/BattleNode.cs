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
            case BattleDifficulty.Boss: return "Icons_map/battle_boss";
            case BattleDifficulty.Elite: return "Icons_map/elita";
            default: return "Icons_map/przeciwnik";
        }
    }
}