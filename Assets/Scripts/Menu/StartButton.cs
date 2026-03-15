using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void onStartButtonClick()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
