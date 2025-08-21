using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUIControllerRE : PopupController
{
    public TMP_Text ruleText;
    public Button nextButton;
    public Button prevButton;

    [TextArea(3, 10)]
    [SerializeField] string[] tutorialSlides;
    
    int currentIndex = 0;

    void Awake()
    {
        nextButton.onClick.AddListener(NextSlide);
        prevButton.onClick.AddListener(PrevSlide);

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
        UpdateSlide();
    }

    private void NextSlide()
    {
        if (currentIndex < tutorialSlides.Length - 1)
        {
            currentIndex++;
            UpdateSlide();
        }
        else
        {
            ClosePopup();
        }
    }

    private void PrevSlide()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateSlide();
        }
        else
        {
            ClosePopup();
        }
    }

    private void UpdateSlide()
    {
        ruleText.text = tutorialSlides[currentIndex];
    }
}
