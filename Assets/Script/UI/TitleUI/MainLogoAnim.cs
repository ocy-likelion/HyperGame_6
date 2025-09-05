using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] RectTransform logo;

    void Start()
    {
        var image = logo.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        logo.GetComponent<Image>().DOFade(1f, 0.5f).SetEase(Ease.Linear);
        logo.DOLocalMoveY(logo.localPosition.y + 25f, 1.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
