using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CardUIClickable : CardUIBase, IPointerClickHandler
{
    public event Action<Card> OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(card);
    }
}