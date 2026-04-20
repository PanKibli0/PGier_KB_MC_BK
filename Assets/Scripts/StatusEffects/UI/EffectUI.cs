using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class EffectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text secondaryText;

    private Tooltip tooltip;
    private BaseStatusEffect singleEffect;
    private List<BaseStatusEffect> hiddenEffects;
    private bool isMoreMode;
    private Sprite cachedIcon;
    private List<(Sprite sprite, string name, string description)> cachedEntries;

    public void init(BaseStatusEffect effect, Tooltip tooltip)
    {
        this.tooltip = tooltip;
        singleEffect = effect;
        isMoreMode = false;

        string iconPath = effect.getIconPath();
        if (!string.IsNullOrEmpty(iconPath))
        {
            cachedIcon = Resources.Load<Sprite>(iconPath);
            if (cachedIcon != null)
            {
                icon.sprite = cachedIcon;
                icon.gameObject.SetActive(true);
            }
        }

        mainText.text = effect.getMainText();
        secondaryText.text = effect.getSecondaryText();
    }

    public void initAsMore(List<BaseStatusEffect> effects, Tooltip tooltip)
    {
        this.tooltip = tooltip;
        hiddenEffects = effects;
        isMoreMode = true;

        mainText.text = "+" + effects.Count;
        secondaryText.text = "";

        cachedEntries = new List<(Sprite, string, string)>();
        foreach (var effect in effects)
        {
            string path = effect.getIconPath();
            Sprite sprite = null;
            if (!string.IsNullOrEmpty(path))
                sprite = Resources.Load<Sprite>(path);
            cachedEntries.Add((sprite, effect.effectName, effect.getDescription()));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isMoreMode)
        {
            tooltip.show(cachedEntries);
        }
        else
        {
            List<(Sprite, string, string)> entries = new List<(Sprite, string, string)>();
            entries.Add((cachedIcon, singleEffect.effectName, singleEffect.getDescription()));
            tooltip.show(entries);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.hide();
    }
}