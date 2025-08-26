using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] RectTransform logo;

    void Start()
    {
        logo.localScale = Vector3.zero;
        logo.DOScale(1f, 1f).SetEase(Ease.OutBack);

        var image = logo.GetComponent<UnityEngine.UI.Image>();
        image.DOFade(0f, 0f).From().SetEase(Ease.Linear);

        logo.DOLocalMoveY(logo.localPosition.y + 25f, 1.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
