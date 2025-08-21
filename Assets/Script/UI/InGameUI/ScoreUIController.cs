using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreUIController : MonoBehaviour
{
    public TMP_Text score;
    public TMP_Text scoreMag;
    [SerializeField] private Image newRecordImage;
    private readonly float _hiddenY = 23f;   // Unity에서 위치를 보고 맞춘 값
    private readonly float _shownY = 58f;   // Unity에서 위치를 보고 맞춘 값

    // New Record 이미지 위치 초기화
    public void InitNewRecordImage()
    {
        Vector2 newRecordPos = newRecordImage.rectTransform.anchoredPosition;
        newRecordPos.y = _hiddenY;
        newRecordImage.rectTransform.anchoredPosition = newRecordPos;
    }

    // New Record Animation
    public void ShowNewRecordImage()
    {
        newRecordImage.rectTransform.DOAnchorPosY(_shownY, 0.3f).SetEase(Ease.OutBack);
    }
}