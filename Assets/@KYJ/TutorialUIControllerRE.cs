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
}

public class TutorialUIControllerRE : PopupController
{
    [Header("UI")]
    public Image tutoImage;
    Button nextButton;
    Image touchImage;

    [Header("텍스트 & 이미지")]
    public TutorialPage[] tutorialPages;

    int currentIndex = 0;
    Tween touchTween;

    void Awake()
    {
        nextButton = transform.parent.Find("TutorialButton").GetComponentInChildren<Button>();
        touchImage = transform.parent.Find("TutorialTouchImage").GetComponentInChildren<Image>();

        nextButton.onClick.AddListener(NextSlide);
        UpdateSlide();
    }

    public void ShowPopup()
    {
        base.ShowPopup(gameObject);
        currentIndex = 0;
        nextButton.gameObject.SetActive(true);
        touchImage.gameObject.SetActive(true);
        UpdateSlide();
        StartTouchEffect();
    }

    public void ClosePopup()
    {
        base.ClosePopup(gameObject);
        currentIndex = 0;
        nextButton.gameObject.SetActive(false);
        touchImage.gameObject.SetActive(false);
        StopTouchEffect();
    }

    void NextSlide()
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

    void UpdateSlide()
    {
        tutoImage.sprite = tutorialPages[currentIndex].image;
        tutoImage.SetNativeSize();
    }


    public void StartTouchEffect()
    {
        if (touchImage == null) return;

        touchTween?.Kill();

        touchTween = touchImage.transform.DOScale(1.7f, 0.8f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopTouchEffect()
    {
        if(touchTween != null && touchTween.IsActive())
        {
            touchTween.Kill();
            touchImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
}
