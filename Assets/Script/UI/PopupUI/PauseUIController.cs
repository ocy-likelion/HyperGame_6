using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIController : PopupController
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _audioToggleButton;
    [SerializeField] private Button _vibrateToggleButton;

    void Awake()
    {
        _resumeButton.onClick.AddListener(OnClickResumeButton);
        _retryButton.onClick.AddListener(OnClickRetryButton);
        _quitButton.onClick.AddListener(OnClickQuitButton);
        _audioToggleButton.onClick.AddListener(OnClickAudioToggleButton);
        _vibrateToggleButton.onClick.AddListener(OnClickVibrateToggleButton);
    }
    
    
    
    public void ShowPopup()
    {
        base.ShowPopup(gameObject);
    }
    
    public void ClosePopup()
    {
        base.ClosePopup(gameObject);
    }
    
    public void OnClickResumeButton()
    {
        ClosePopup();
        GameManager.Instance.ResumeGame();
    }
    
    public void OnClickRetryButton()
    {
        GameManager.Instance.GoToInGame();
        ClosePopup();
    }
    
    public void OnClickQuitButton()
    {
        ClosePopup();
        GameManager.Instance.ResumeGame();
        GameManager.Instance.inGameController.Dispose();
        GameManager.Instance.inGameController.QuitGame();
    }
    
    public void OnClickAudioToggleButton()
    {
        
    }
    
    public void OnClickVibrateToggleButton()
    {
        
    }
}
