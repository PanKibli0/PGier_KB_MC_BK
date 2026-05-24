using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventChoiceButton : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;

    private EventChoice choice;
    private EventSceneManager manager;

    public void init(EventChoice choice, EventSceneManager manager)
    {
        this.choice = choice;
        this.manager = manager;

        buttonText.text = choice.buttonText;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClick);
    }

    private void onClick()
    {
        manager.selectChoice(choice);
    }
}