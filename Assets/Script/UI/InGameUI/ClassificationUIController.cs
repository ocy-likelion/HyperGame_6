using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassificationUIController : MonoBehaviour
{
    [SerializeField] private Image greenBox;
    [SerializeField] private Image redBox;

    private Tweener effectTweener;

    private void Awake()
    {
        InitBox(greenBox);
        InitBox(redBox);
    }

    private void InitBox(Image box)
    {
        if (box != null)
        {
            var c = box.color;
            c.a = 0f; // 시작 시 투명
            box.color = c;
            box.raycastTarget = false; // UI 클릭 방지
        }
    }

    public void TriggerSuccessEffect()
    {
        TriggerEffect(greenBox);
    }

    public void TriggerFailEffect()
    {
        TriggerEffect(redBox);
    }

    private void TriggerEffect(Image box)
    {
        if (box == null) return;

        // 기존 트윈 정지
        effectTweener?.Kill();

        // 알파를 일정 값(예: 0.3)으로 세팅
        var c = box.color;
        c.a = 0.5f;
        box.color = c;

        // 깜빡이고 투명해지도록
        effectTweener = box.DOFade(0f, 0.5f).OnComplete(() =>
        {
            effectTweener = null;
        });
    }
}
