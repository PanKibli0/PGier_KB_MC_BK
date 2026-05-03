using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestSceneUI : MonoBehaviour
{
    [SerializeField] private Button healButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private RestCardUpgradePanel upgradePanel;

    private bool hasUsedAction = false;

    public void onHeal()
    {
        if (hasUsedAction) return;

        int healAmount = Mathf.RoundToInt(GameManager.Instance.maxHealth * 0.3f);
        GameManager.Instance.setHealth(GameManager.Instance.currentHealth + healAmount);

        hasUsedAction = true;
        Destroy(healButton.gameObject);
        Destroy(upgradeButton.gameObject);
    }

    public void onUpgrade()
    {
        if (hasUsedAction) return;
        mainPanel.SetActive(false);
        upgradePanel.gameObject.SetActive(true);
        upgradePanel.show();
    }

    public void onUpgradeCompleted()
    {
        hasUsedAction = true;
        Destroy(healButton.gameObject);
        Destroy(upgradeButton.gameObject);
        upgradePanel.gameObject.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void onUpgradeCancelled()
    {
        upgradePanel.gameObject.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void onContinue()
    {
        GameManager.Instance.currentMapNode.onComplete();
        SceneManager.LoadScene("MapScene");
    }
}