using UnityEngine;

public class RelicPanel : MonoBehaviour
{
    [SerializeField] private Transform relicContainer;
    [SerializeField] private GameObject relicUIPrefab;
    [SerializeField] private Tooltip tooltip;

    private RelicManager relicManager;

    private void Start()
    {
        relicManager = GameManager.Instance.relicManager;

        if (relicManager != null)
        {
            relicManager.onRelicsChanged += refresh;
            refresh();
        }
    }

    private void OnDestroy()
    {
        if (relicManager != null)
            relicManager.onRelicsChanged -= refresh;
    }

    private void refresh()
    {
        foreach (Transform child in relicContainer)
            Destroy(child.gameObject);

        foreach (var relic in relicManager.getRelics())
        {
            GameObject relicObj = Instantiate(relicUIPrefab, relicContainer);
            relicObj.GetComponent<RelicUI>().setup(relic, tooltip);
        }
    }
}