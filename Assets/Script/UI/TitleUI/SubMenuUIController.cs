using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuUIController : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _gameSettingButton;
    [SerializeField] private Button _leaderBoardButton;
    [SerializeField] private Sprite[] _swapImage = new Sprite[2];
    private Sprite[] _audioBtnSprites = new Sprite[2];
    private bool _isClicked = false;

    private void Awake()
    {
        //버튼 클릭이벤트 등록
        _gameSettingButton.onClick.AddListener(OnClickGameSettingButton);
        _leaderBoardButton.onClick.AddListener(OnClickLeaderBoardButton);
    }
    
    public async Task LoadSprites()
    {
        var audioBtnSprites = await DataManager.Instance.LoadSpritesData(Addresses.Sprites.Buttons.Sounds.OnOff);
        _audioBtnSprites[0] = audioBtnSprites[0];
        _audioBtnSprites[1] = audioBtnSprites[1];
        _gameSettingButton.image.sprite = _audioBtnSprites[0];
    }
    
    public void InitUI()
    {
        _gameSettingButton.image.sprite = AudioManager.Instance.GetIsAudioOn() ? _audioBtnSprites[0] : _audioBtnSprites[1];
    }
    
    public void OnClickGameSettingButton()
    {
        if (!_isClicked)
        {
            _gameSettingButton.image.sprite = _swapImage[1];
            _isClicked = true;
        }
        else
        {
            _gameSettingButton.image.sprite = _swapImage[0];
            _isClicked = false;
        }
        AudioManager.Instance.ToggleAudio();
        _gameSettingButton.image.sprite = AudioManager.Instance.GetIsAudioOn() ? _audioBtnSprites[0] : _audioBtnSprites[1];
    }

    public void OnClickLeaderBoardButton()
    {
        Debug.Log("OnClickLeaderBoardButton");
        AudioManager.Instance.SFX.PlayButtonClick();
        
        //토스 게임 리더보드 열어보기
        NetworkManager.Instance.OnTossLeaderboard();
    }
}
