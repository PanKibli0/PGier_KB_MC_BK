using System.Collections.Generic;
using UnityEngine;

public class RelicRewardPanel : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject relicPrefab;
    [SerializeField] private GameObject rewardsList;
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private float relicScale = 2.5f;

    private RelicReward reward;

    public void setRelics(List<RelicData> relics, RelicReward reward)
    {
        this.reward = reward;

        foreach (Transform child in container)
            Destroy(child.gameObject);

        foreach (var relic in relics)
        {
            GameObject obj = Instantiate(relicPrefab, container);
            obj.transform.localScale = Vector3.one * relicScale;
            obj.GetComponent<RelicUI>().setupReward(relic, this, tooltip);
        }
    }

    public void selectRelic(RelicData relic)
    {
        GameManager.Instance.relicManager.addRelic(relic);
        reward.complete();
        gameObject.SetActive(false);
        rewardsList.SetActive(true);
    }

    public void onCloseButton()
    {
        rewardsList.SetActive(true);
        gameObject.SetActive(false);
    }
}