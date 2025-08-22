using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

[System.Serializable]
public class TutorialPage
{
    public Sprite image;
    public Sprite explain;
}

public class TutorialUIControllerRE : PopupController
{
    [Header("UI")]
    public Image mainImage;
    public Image explainImage;
    public Button nextButton;
    public Image touchImage;

    [Header("DOTween 이펙트")]
    public float scale;
    public float delay;

    [Header("텍스트 & 이미지")]
    public TutorialPage[] tutorialPages;

    int currentIndex = 0;
    Tween touchTween;

    void Awake()
    {
        nextButton.onClick.AddListener(NextSlide);
        UpdateSlide();
    }

    public void ShowPopup()
    {
        base.ShowPopup(gameObject);
        currentIndex = 0;
        UpdateSlide();
        StartTouchEffect();
    }

    public void ClosePopup()
    {
        base.ClosePopup(gameObject);
        currentIndex = 0;
        StopTouchEffect();
    }

    private void NextSlide()
    {
        if (currentIndex < tutorialPages.Length - 1)
        {
            currentIndex++;
            UpdateSlide();
        }
        else
        {
            ClosePopup();
        }
    }

    private void UpdateSlide()
    {
        mainImage.sprite = tutorialPages[currentIndex].image;
        explainImage.sprite = tutorialPages[currentIndex].explain;
        mainImage.SetNativeSize();
        explainImage.SetNativeSize();
    }


    public void StartTouchEffect()
    {
        if (touchImage == null) return;

        touchTween?.Kill();

        touchTween = touchImage.transform.DOScale(scale, delay).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopTouchEffect()
    {
        if(touchTween != null && touchTween.IsActive())
        {
            touchTween.Kill();
            touchImage.transform.localScale = new Vector3(2,2,2);
        }
    }
}
