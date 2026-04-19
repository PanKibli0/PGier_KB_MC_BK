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

    public void init(BaseStatusEffect effect, Tooltip tooltip)
    {
        this.tooltip = tooltip;
        singleEffect = effect;
        isMoreMode = false;

        if (effect.icon != null)
        {
            icon.sprite = effect.icon;
            icon.gameObject.SetActive(true);
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

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        List<(Sprite, string, string)> entries = new List<(Sprite, string, string)>();

        if (isMoreMode)
        {
            foreach (var effect in hiddenEffects)
                entries.Add((effect.icon, effect.effectName, effect.getDescription()));
        }
        else
            entries.Add((singleEffect.icon, singleEffect.effectName, singleEffect.getDescription()));

        tooltip.show(entries);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.hide();
    }
}