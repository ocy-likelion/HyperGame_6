using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private Sprite[] _audioBtnSprites = new Sprite[2];

    void Awake()
    {
        _resumeButton.onClick.AddListener(OnClickResumeButton);
        _retryButton.onClick.AddListener(OnClickRetryButton);
        _quitButton.onClick.AddListener(OnClickQuitButton);
        _audioToggleButton.onClick.AddListener(OnClickAudioToggleButton);
    }

    public async Task LoadSprites()
    {
        var audioBtnSprites = await DataManager.Instance.LoadSpritesData(Addresses.Sprites.Buttons.Sounds.OnOff);
        _audioBtnSprites[0] = audioBtnSprites[0];
        _audioBtnSprites[1] = audioBtnSprites[1];
        _audioToggleButton.image.sprite = _audioBtnSprites[0];
    }
    
    public void ShowPopup()
    {
        _audioToggleButton.image.sprite = AudioManager.Instance.GetIsAudioOn() ? _audioBtnSprites[0] : _audioBtnSprites[1];
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
        GameManager.Instance.ResumeGame();
        GameManager.Instance.inGameController.Dispose();
        GameManager.Instance.inGameController.UseRetry();
        GameManager.Instance.inGameController.SkipResultUI();
        GameManager.Instance.inGameController.QuitGame();
        ClosePopup();
    }
    
    public void OnClickQuitButton()
    {
        ClosePopup();
        GameManager.Instance.ResumeGame();
        GameManager.Instance.inGameController.SkipResultUI();
        GameManager.Instance.inGameController.Dispose();
        GameManager.Instance.inGameController.QuitGame();
    }
    
    public void OnClickAudioToggleButton()
    {
        AudioManager.Instance.ToggleAudio();
        _audioToggleButton.image.sprite = AudioManager.Instance.GetIsAudioOn() ? _audioBtnSprites[0] : _audioBtnSprites[1];
    }
}
