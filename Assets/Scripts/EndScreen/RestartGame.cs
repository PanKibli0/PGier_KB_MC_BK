using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void RestartGameScene()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void LoadStatsScene()
    {
        SceneManager.LoadScene("EndScreenScene");
    }
}