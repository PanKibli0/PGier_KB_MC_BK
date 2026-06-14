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
    private void loadEvent(EventData newEvent)
    {
        currentEvent = newEvent;

        foreach (Transform child in choicesParent)
            Destroy(child.gameObject);

        resultPanel.SetActive(false);

        setupEvent();
    }

    public void selectChoice(EventChoice choice)
    {
        EventContext context = new EventContext(GameManager.Instance);

        foreach (var effect in choice.effects)
        {
            switch (effect.type)
            {
                case EventEffectType.AddGold:
                    GameManager.Instance.addGold(effect.intValue);
                    break;

                case EventEffectType.HealPlayer:
                    GameManager.Instance.setHealth(GameManager.Instance.currentHealth + effect.intValue);
                    break;

                case EventEffectType.TakeDamage:
                    GameManager.Instance.setHealth(GameManager.Instance.currentHealth - effect.intValue);
                    break;

                case EventEffectType.AddRelic:
                    GameManager.Instance.relicManager.addRelic(effect.relic);
                    break;

                case EventEffectType.StartBattle:
                    {
                        var pool = GameManager.Instance.enemyPool;

                        var fight = pool.normalFights[UnityEngine.Random.Range(0, pool.normalFights.Count)];

                        GameManager.Instance.pendingBattleEnemies = fight.enemies;
                        GameManager.Instance.pendingBattleDifficulty = BattleDifficulty.Normal;

                        GameManager.Instance.currentMapNode?.onComplete();
                        GameManager.Instance.currentEvent = null;
                        SceneManager.LoadScene("BattleScene");
                        return;
                    }
            }
        }
        if (choice.nextEvent != null)
        {
            loadEvent(choice.nextEvent);
            return;
        }

        resultPanel.SetActive(true);
        resultText.text = choice.resultText;
    }

    public void leaveEvent()
    {
        GameManager.Instance.currentMapNode?.onComplete();
        SceneManager.LoadScene("MapScene");
    }
}