using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventSceneManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image image;

    [SerializeField] private Transform choicesParent;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Result")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;

    private EventData currentEvent;

    void Start()
    {
        currentEvent = GameManager.Instance.currentEvent;

        setupEvent();
    }

    void setupEvent()
    {
        titleText.text = currentEvent.eventName;
        descriptionText.text = currentEvent.description;

        if (currentEvent.illustration != null)
            image.sprite = currentEvent.illustration;

        foreach (var choice in currentEvent.choices)
        {
            GameObject obj = Instantiate(choiceButtonPrefab, choicesParent);

            EventChoiceButton btn = obj.GetComponent<EventChoiceButton>();
            btn.init(choice, this);
        }
    }

    public void selectChoice(EventChoice choice)
    {
        foreach (var action in choice.actions)
        {
            action.execute(
                UnitsManager.Instance.player,
                UnitsManager.Instance.player
            );
        }

        resultPanel.SetActive(true);
        resultText.text = choice.resultText;
    }

    public void leaveEvent()
    {
        Debug.Log("POMIÑ KLIK");
        GameManager.Instance.currentMapNode?.onComplete();
        SceneManager.LoadScene("MapScene");
    }
}