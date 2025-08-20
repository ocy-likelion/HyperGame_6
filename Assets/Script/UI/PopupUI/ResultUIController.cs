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
    [SerializeField] private Image newRecordImage;

    void Awake()
    {
        _quitButton.onClick.AddListener(OnClickQuitButton);
        if (newRecordImage != null)
            newRecordImage.gameObject.SetActive(false);
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
                ShowNewRecordEffect();
                // New Record 체크
                if (resultData.Score > bestScore)
                {
                    PlayerPrefs.SetFloat("BestScore", resultData.Score);
                }
            }));
    }

    // New Record 시, 효과
    public void ShowNewRecordEffect()
    {
        newRecordImage.gameObject.SetActive(true);
        
        // 초기화 (작고 안 보이는 상태)
        newRecordImage.color = new Color(1f, 1f, 1f, 0f);
        newRecordImage.rectTransform.localScale = Vector3.zero * 0.8f;
        
        Sequence seq = DOTween.Sequence();
        // Fade In + Scale Up
        seq.Append(newRecordImage.DOFade(1f, 0.5f));
        seq.Join(newRecordImage.rectTransform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack));
        // 살짝 튕기면서 원래 크기로
        seq.Append(newRecordImage.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        // 착!
        seq.Append(newRecordImage.rectTransform.DOScale(0.95f, 0.1f).SetEase(Ease.InQuad));
        seq.Append(newRecordImage.rectTransform.DOScale(1f, 0.15f).SetEase(Ease.OutQuad));
        
        // 
        seq.OnComplete(() =>
        {
            
        });
    }

    public void OnClickQuitButton()
    {
        ClosePopup();
        GameManager.Instance.ResumeGame();
        GameManager.Instance.inGameController.QuitGame();
    }
}
