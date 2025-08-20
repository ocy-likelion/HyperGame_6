using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassificationUIController : MonoBehaviour
{
    [SerializeField] private Image greenBox;
    [SerializeField] private Image redBox;
    
    private Tweener effectTweener;

    [SerializeField] private TMP_Text _timerEffectText;
    private RectTransform _timerEffectRect;
    private Vector2 _timerEffectStartAnchoredPos;
    
    private void Awake()
    {
        InitBox(greenBox);
        InitBox(redBox);
        
        _timerEffectRect = _timerEffectText.GetComponent<RectTransform>();
        _timerEffectStartAnchoredPos = _timerEffectRect.anchoredPosition; // 시작 위치 
        Debug.Log(_timerEffectStartAnchoredPos);
    }

    private void InitBox(Image box)
    {
        if (box != null)
        {
            var c = box.color;
            c.a = 0f; // ���� �� ����
            box.color = c;
            box.raycastTarget = false; // UI Ŭ�� ����
        }
    }

    public void TriggerSuccessEffect()
    {
        TriggerEffect(greenBox);
        RisingEffect(true);
    }

    public void TriggerFailEffect()
    {
        TriggerEffect(redBox);
        RisingEffect(false);
    }

    private void TriggerEffect(Image box)
    {
        if (box == null) return;

        // ���� Ʈ�� ����
        effectTweener?.Kill();

        // ���ĸ� ���� ��(��: 0.3)���� ����
        var c = box.color;
        c.a = 0.5f;
        box.color = c;

        // �����̰� ������������
        effectTweener = box.DOFade(0f, 0.5f).OnComplete(() =>
        {
            effectTweener = null;
        });
    }

    private void RisingEffect(bool isCorrect)
    {
        // 트윈 정리 (기존 효과 즉시 끊기)
        _timerEffectRect.DOKill(true);   // true = 마지막 값을 적용하고 종료
        _timerEffectText.DOKill(true);

        // 초기화
        _timerEffectText.gameObject.SetActive(true);
        _timerEffectText.text = isCorrect ? "+ 시간 증가!!" : "- 시간 감소...";
        _timerEffectText.color = isCorrect ? Color.green : Color.red;

        // 알파값 강제 세팅
        var col = _timerEffectText.color;
        col.a = 1f;
        _timerEffectText.color = col;

        // 위치 초기화
        _timerEffectRect.anchoredPosition = _timerEffectStartAnchoredPos;

        // 새 연출 시작 (OnComplete 안 씀)
        _timerEffectRect.DOAnchorPosY(_timerEffectStartAnchoredPos.y + 50f, 1f);
        _timerEffectText.DOFade(0f, 1f);
    }
}
