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

        Sprite sprite = Resources.Load<Sprite>(effect.iconPath);
        if (sprite != null)
        {
            icon.sprite = sprite;
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
            {
                Sprite sprite = Resources.Load<Sprite>(effect.iconPath);
                entries.Add((sprite, effect.effectName, effect.getDescription()));
            }
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>(singleEffect.iconPath);
            entries.Add((sprite, singleEffect.effectName, singleEffect.getDescription()));
        }

        tooltip.show(entries);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.hide();
    }
}
