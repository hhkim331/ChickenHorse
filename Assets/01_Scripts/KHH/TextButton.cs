using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButton : Button
{
    TextMeshProUGUI[] texts;

    protected override void Start()
    {
        base.Start();
        texts = GetComponentsInChildren<TextMeshProUGUI>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        foreach(var text in texts)
            text.fontStyle = FontStyles.Bold;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        foreach(var text in texts)
            text.fontStyle = FontStyles.Normal;
    }
}
