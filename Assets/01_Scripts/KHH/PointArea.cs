using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointArea : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] Image pointImage;
    [SerializeField] TextMeshProUGUI pointText;

    [SerializeField] Sprite[] sprites;

    float areaMulWidth = 40;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    public void Set(int start, Point.PointType type, Point point)
    {
        //세팅
        rect.pivot = new Vector2(0f, 0.5f);
        rect.sizeDelta = new Vector2(0, 150);
        rect.localPosition = new Vector3(start * areaMulWidth, 0, 0);

        pointImage.rectTransform.pivot = new Vector2(0f, 0.5f);
        pointImage.rectTransform.sizeDelta = new Vector2(point.point * areaMulWidth, 150);
        pointImage.rectTransform.localPosition = Vector3.zero;
        if (type == Point.PointType.Goal)
            pointImage.sprite = sprites[0];
        else
            pointImage.sprite = sprites[1];
        pointImage.color = point.color;

        pointText.text = point.name;
        pointText.color = new Color(point.color.r, point.color.g, point.color.b, 0);
        pointText.rectTransform.localPosition = new Vector3(point.point * areaMulWidth + 20, 0, 0);

        //애니메이션
        rect.DOSizeDelta(new Vector2(point.point * areaMulWidth, 150), 0.5f).SetEase(Ease.OutCubic);
        pointText.DOFade(1, 0.5f).SetEase(Ease.OutCubic).OnComplete(() => { pointText.DOFade(0, 0.5f).SetEase(Ease.InCubic); });
    }
}
