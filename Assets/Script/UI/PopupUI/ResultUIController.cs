using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultUIController : PopupController
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _quitButton;

    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text maxComboText;
    [SerializeField] private TMP_Text scoreText;

    [Header("New Record UI")]
    [SerializeField] private TMP_Text newRecordText;

    void Awake()
    {
        _quitButton.onClick.AddListener(OnClickQuitButton);
        if (newRecordText != null)
            newRecordText.gameObject.SetActive(false);
    }
    
    public void ShowPopup()
    {
        base.ShowPopup(gameObject);
    }
    
    public void ClosePopup()
    {
        base.ClosePopup(gameObject);
    }
    
    public void InitResultItem(GameResultData resultData)
    {
        // 처음에는 0으로 초기화
        dayText.text = "0";
        maxComboText.text = "0";
        scoreText.text = "0";
        
        // TODO: 유저의 최고기록 불러오기 (임시: PlayerPrefs)
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        
        Sequence seq = DOTween.Sequence();
        
        // Day Count Up
        seq.Append(DOTween.To(() => 0, x => dayText.text = x.ToString() + "일", resultData.Day, 1f));
        seq.AppendInterval(0.2f);
        
        // MaxCombo Count Up
        seq.Append(DOTween.To(() => 0, x => maxComboText.text = x.ToString(), resultData.MaxCombo, 1f));
        seq.AppendInterval(0.2f);

        // Score Count Up
        seq.Append(DOTween.To(() => 0, x => scoreText.text = x.ToString(), resultData.Score, 1.5f)
            .OnComplete(() =>
            {
                // New Record 체크
                if (resultData.Score > bestScore)
                {
                    PlayerPrefs.SetInt("BestScore", resultData.Score);
                    ShowNewRecordEffect();
                }
            }));
    }

    // New Record 시, 효과
    public void ShowNewRecordEffect()
    {
        if (newRecordText != null)
        {
            newRecordText.gameObject.SetActive(true);
            newRecordText.alpha = 0f;
            newRecordText.transform.localScale = Vector3.zero * 0.8f;
            
            Sequence seq = DOTween.Sequence();
            // Fade In
            seq.Append(newRecordText.DOFade(1f, 0.5f));
            // 글자가 살짝 커졌다가 원래 크기로
            seq.Join(newRecordText.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack));
            seq.Append(newRecordText.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine));
            // 번쩍거리는 효과 무한 반복
            seq.OnComplete(() =>
            {
                newRecordText.DOFade(0.3f, 0.2f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
            });
        }
    }

    public void OnClickQuitButton()
    {
        ClosePopup();
        GameManager.Instance.ResumeGame();
        GameManager.Instance.inGameController.QuitGame();
    }
}
