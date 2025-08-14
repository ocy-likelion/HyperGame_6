using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameResultData
{
    public int Day;
    public int MaxCombo;
    public int Score;
     
    public GameResultData(int day, int maxCombo, int score)
    {
        Day = day;
        MaxCombo = maxCombo;
        Score = score;
    }
}

public class ResultUIController : PopupController
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _quitButton;

    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text maxComboText;
    [SerializeField] private TMP_Text scoreText;

    void Awake()
    {
        _quitButton.onClick.AddListener(OnClickQuitButton);
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
        dayText.text = resultData.Day.ToString();
        maxComboText.text = resultData.MaxCombo.ToString();
        scoreText.text = resultData.Score.ToString();
    }

    public void OnClickQuitButton()
    {
        ClosePopup();
        GameManager.Instance.ResumeGame();
        GameManager.Instance.inGameController.QuitGame();
    }
}
