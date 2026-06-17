using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RelicSceneManager : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject relicPrefab;
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private float relicScale = 2.5f;

    private void Start()
    {
        List<RelicData> pool = new List<RelicData>(GameManager.Instance.relicPool.relics);

        List<RelicData> chosen = new List<RelicData>();
        int count = Mathf.Min(3, pool.Count);
        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, pool.Count);
            chosen.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        foreach (var relic in chosen)
        {
            GameObject obj = Instantiate(relicPrefab, container);
            obj.transform.localScale = Vector3.one * relicScale;
            obj.GetComponent<RelicUI>().setupScene(relic, this, tooltip);
        }
    }

    public void selectRelic(RelicData relic)
    {
        GameManager.Instance.relicManager.addRelic(relic);
        GameManager.Instance.currentMapNode.onComplete();
        SceneManager.LoadScene("MapScene");
    }

    public void onSkip()
    {
        GameManager.Instance.currentMapNode.onComplete();
        SceneManager.LoadScene("MapScene");
    }
}