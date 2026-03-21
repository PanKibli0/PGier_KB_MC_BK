using UnityEngine;
using UnityEngine.UI;

public class HandAreaUI : MonoBehaviour
{
    public void RefreshLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}