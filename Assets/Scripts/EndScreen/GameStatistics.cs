using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    public int enemiesKilled;
    public int floorsCompleted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetStats()
    {
        enemiesKilled = 0;
        floorsCompleted = 0;
    }
}