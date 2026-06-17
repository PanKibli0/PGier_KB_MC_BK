using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void onStartButtonClick()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadScene("CharacterSelectScene");
    }
}
