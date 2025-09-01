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
    [SerializeField] private Button _retryButton;

    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text maxComboText;
    [SerializeField] private TMP_Text scoreText;
    
    [SerializeField] private Image newRecordImage;
    [SerializeField] private CanvasGroup fadeOutCanvasGroup;
    
    public Image errorCheckImage;

    void Awake()
    {
        _quitButton.onClick.AddListener(OnClickQuitButton);
        _retryButton.onClick.AddListener(OnClickRetryButton);
        errorCheckImage.gameObject.SetActive(false);
    }
    
    public void ShowPopup()
    {
        base.ShowPopup(gameObject);
    }
    
    public void ClosePopup()
    {
        base.ClosePopup(gameObject);
        errorCheckImage.gameObject.SetActive(false);
    }
    
    public void InitResultItem(GameResultData resultData)
    {
        // 점수 보내기
        //해당기능에서는 점수를 string 타입으로 받음. 임시로 정수 형변환을 시켰지만
        //추후 반올림같은 로직을 넣는다면 그렇게 한 결과값을 인수로 넣도록 수정할 것.
        NetworkManager.Instance.SendScore((int)resultData.Score);
        
        // FadeOut Panel 초기화
        fadeOutCanvasGroup.alpha = 0;
        
        // 퇴근 및 재시작 버튼 비활성화
        _quitButton.gameObject.SetActive(false);
        _retryButton.gameObject.SetActive(false);
        
        // New Record 이미지 비활성화
        newRecordImage.gameObject.SetActive(false);
        
        // BGM 볼륨을 절반으로 설정
        AudioManager.Instance.BGM.SetBGMVolumeHalf();
        
        // 처음에는 0으로 초기화
        dayText.text = "0";
        maxComboText.text = "0";
        scoreText.text = "0";
        
        // TODO: 유저의 최고기록 불러오기 (임시: PlayerPrefs)
        float bestScore = PlayerPrefs.GetFloat("BestScore", 0f);
        
        AudioManager.Instance.SFX.PlayScoreCalculating();
        Sequence seq = DOTween.Sequence();
        
        // Day Count Up
        seq.Append(DOTween.To(() => 0, x => dayText.text = x.ToString() + "일", resultData.Day, 1f)
            .OnComplete(() =>
            {
                AudioManager.Instance.SFX.PlayScoreCalculated();
            }));
        seq.AppendInterval(0.2f);
        
        // MaxCombo Count Up
        seq.Append(DOTween.To(() => 0, x => maxComboText.text = x.ToString(), resultData.MaxCombo, 1f)
            .OnComplete(() =>
            {
                AudioManager.Instance.SFX.PlayScoreCalculated();
            }));
        seq.AppendInterval(0.2f);

        // Score Count Up
        seq.Append(DOTween.To(() => 0, x => scoreText.text = x.ToString("N0"), resultData.Score, 1.5f)
            .OnComplete(() =>
            {
                AudioManager.Instance.SFX.StopScoreCalculating();
                AudioManager.Instance.SFX.PlayScoreCalculated();
                // 퇴근 및 재시작 버튼 활성화
                _quitButton.gameObject.SetActive(true);
                _retryButton.gameObject.SetActive(true);
                
                // New Record 체크
                if (resultData.Score > bestScore)
                {
                    PlayerPrefs.SetFloat("BestScore", resultData.Score);
                    PlayerPrefs.Save();
                    ShowNewRecordEffect();
                }
            }));
    }

    // New Record 시, 효과
    private void ShowNewRecordEffect()
    {
        newRecordImage.gameObject.SetActive(true);
        
        // 초기화 (작고 안 보이는 상태)
        newRecordImage.color = new Color(1f, 1f, 1f, 0f);
        newRecordImage.rectTransform.localScale = Vector3.zero * 0.8f;
        
        Sequence seq = DOTween.Sequence();
        // Fade In + Scale Up
        seq.Append(newRecordImage.DOFade(1f, 0.5f));
        seq.Join(newRecordImage.rectTransform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack));
        // SFX 재생
        seq.AppendCallback(() =>
        {
            AudioManager.Instance.SFX.PlayNewRecordResult();
        });
        // 살짝 튕기면서 원래 크기로
        seq.Append(newRecordImage.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        // 착! 강조
        seq.Append(newRecordImage.rectTransform.DOScale(0.95f, 0.1f).SetEase(Ease.InQuad));
        seq.Append(newRecordImage.rectTransform.DOScale(1f, 0.15f).SetEase(Ease.OutQuad));
    }

    public void OnClickQuitButton()
    {
        // 더 이상 클릭되지 않게 막기
        _quitButton.interactable = false;

        // Title로 가기 전, FadeOut
        fadeOutCanvasGroup.DOFade(1f, 1f)
            .OnComplete(() =>
            {
                ClosePopup();
                GameManager.Instance.ResumeGame();
                GameManager.Instance.inGameController.QuitGame();
                _quitButton.interactable = true;
            });
    }

    public void OnClickRetryButton()
    {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.inGameController.Dispose();
        GameManager.Instance.inGameController.UseRetry();
        GameManager.Instance.inGameController.SkipResultUI();
        GameManager.Instance.inGameController.QuitGame();
        ClosePopup();
    }
}