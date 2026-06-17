using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private GameObject continueButtonObject;

    void Start()
    {
        continueButtonObject.SetActive(SaveSystem.saveExists());
    }

    public void onContinueButtonClick()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        if (SaveSystem.load(GameManager.Instance))
            SceneManager.LoadScene("MapScene");
    }
}