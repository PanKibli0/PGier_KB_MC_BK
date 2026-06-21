using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void RestartGameScene()
    {
        if (MainBar.Instance != null)
            Destroy(MainBar.Instance.gameObject);

        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void LoadStatsScene()
    {
        SceneManager.LoadScene("EndScreenScene");
    }
}