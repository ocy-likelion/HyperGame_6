using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TutorialPage
{
    [TextArea(3, 10)]
    public string text;
    public Sprite image;
}

public class TutorialUIControllerRE : PopupController
{
    [Header("UI")]
    public TMP_Text ruleText;
    public Image ruleImage;
    public Button nextButton;

    [Header("텍스트 & 이미지")]
    public TutorialPage[] tutorialPages;
    
    int currentIndex = 0;

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
    }

    public void ClosePopup()
    {
        base.ClosePopup(gameObject);
        currentIndex = 0;
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
        ruleText.text = tutorialPages[currentIndex].text;
        ruleImage.sprite = tutorialPages[currentIndex].image;
        ruleImage.SetNativeSize();
    }
}
