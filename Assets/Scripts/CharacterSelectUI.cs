using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private CharacterData[] characters;

    private CharacterData selectedCharacter;


    public void selectCharacter(int index)
    {
        selectedCharacter = characters[index];
        startButton.interactable = true;
    }

    public void startGame()
    {
        if (selectedCharacter == null) return;
        GameManager.Instance.startNewRun(selectedCharacter);
        SceneManager.LoadScene("BattleScene");
    }
}