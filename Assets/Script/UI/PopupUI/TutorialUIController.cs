using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUIController : PopupController
{
    [SerializeField] TMP_Text ruleText;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] Button closeButton;

    string[] tutorialSlides;
    int currentIndex = 0;

    void Awake()
    {
        tutorialSlides = new string[]
        {
            "서류를 보고\n올바른 버튼을 누르세요.\n깨끗하면 통과\n더러우면 반려",
            "서류 위 장애물을\n터치해서 제거하세요.\n",
            "잘못된 도장을 찍거나\n장애물을 무시하면\n시간이 줄어듭니다.\n",
            "올바른 도장을 찍으면\n시간이 늘어나고\n피버 게이지가 쌓입니다.\n",
            "Fever 게이지가 가득 차면\n일정시간 동안\n모든 서류가 통과됩니다."
        };

        nextButton.onClick.AddListener(NextSlide);
        prevButton.onClick.AddListener(PrevSlide);
        closeButton.onClick.AddListener(ClosePopup);

        UpdateSlide();
    }

    public void ShowPopup()
    {
        base.ShowPopup(gameObject);
        currentIndex = 0;
        UpdateSlide();
        //gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        base.ClosePopup(gameObject);
        currentIndex = 0;
        UpdateSlide();
        //gameObject.SetActive(false);
    }

    private void NextSlide()
    {
        if (currentIndex < tutorialSlides.Length - 1)
        {
            currentIndex++;
            UpdateSlide();
        }
    }

    private void PrevSlide()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateSlide();
        }
    }

    private void UpdateSlide()
    {
        ruleText.text = tutorialSlides[currentIndex];
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < tutorialSlides.Length - 1;
    }
}
