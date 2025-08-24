using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DifficultyUpEffectUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text effectText;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private Vector3 _originVector;
    private Vector3 _startVector;
    private Vector3 _endVector;
    
    public void Awake()
    {
        _originVector = effectText.transform.position;
        _startVector = _originVector + new Vector3(-2000, 0,0);
        _endVector = _originVector + new Vector3(2000, 0,0);
        canvasGroup.alpha = 0;
        effectText.transform.position = _startVector;
    }

    public void Initialize()
    {
        DifficultyManager.OnLevelChanged += CallLevelUpEffect;
    }

    public void CallLevelUpEffect()
    {
        StartCoroutine(ShowEffectUI());
    }

    public IEnumerator ShowEffectUI()
    {
        var tr = effectText.transform;
        canvasGroup.DOFade(1, 0.1f).OnComplete(() =>
        {
            SFXController.Instance.PlaySpeedUp();
            tr.DOMove(_startVector, 0);
            tr.DOMove(_originVector, 0.35f).SetEase(Ease.OutBounce);
            DOVirtual.DelayedCall(2f, () =>
            {
                tr.DOMove(_endVector, 0.15f).SetEase(Ease.OutExpo).OnComplete(() =>
                {
                    canvasGroup.DOFade(0, 0.1f);
                });
            });
        });
        yield return null;
    }
}
