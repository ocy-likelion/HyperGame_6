using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject panel;
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
            "서류를 보고 올바른 버튼을 누르세요.\n깨끗하면 '통과' / 더러우면 '반려'",
            "서류 위 장애물을 터치해서 제거하세요.\n(벌레, 손, 포스트잇, 파일철 등)",
            "잘못된 도장을 찍거나 장애물을 무시하면 시간이 깎입니다.",
            "정확히 찍으면 시간과 점수 증가!\nFever 게이지를 가득 채우면,\n일정 시간 동안 더러운 서류도 자동 통과가 됩니다."
        };

        nextButton.onClick.AddListener(NextSlide);
        prevButton.onClick.AddListener(PrevSlide);
        closeButton.onClick.AddListener(CloseTutorial);

        UpdateSlide();
    }

    public void OpenTutorial()
    {
        currentIndex = 0;
        UpdateSlide();
        panel.SetActive(true);
    }

    private void CloseTutorial()
    {
        currentIndex = 0;
        UpdateSlide();
        panel.SetActive(false);
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
