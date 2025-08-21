using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuUIController : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _gameSettingButton;
    [SerializeField] private Button _leaderBoardButton;
    [SerializeField] private Sprite[] _swapImage = new Sprite[2];
    private bool _isClicked = false;

    private void Awake()
    {
        //버튼 클릭이벤트 등록
        _gameSettingButton.onClick.AddListener(OnClickGameSettingButton);
        _leaderBoardButton.onClick.AddListener(OnClickLeaderBoardButton);
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
    }

    public void OnClickLeaderBoardButton()
    {
        Debug.Log("OnClickLeaderBoardButton");
        AudioManager.Instance.SFX.PlayButtonClick();
        
        // //점수를 보내는 함수.
        // var gameInfo = new GameInfo { score = 123 };
        // NetworkManager.Instance.SendScore(gameInfo, () =>{
        //         Debug.Log("Send Success");
        //     },
        //     () => {
        //         Debug.Log("Send Fail");
        //     });
        
        //점수를 받는 함수.
        NetworkManager.Instance.RecieveScore((gameInfo) =>{
                Debug.Log($"Send Success = {gameInfo.score}");
                VfxManager.Instance.GetVFX(VFXType.TEST, new Vector2(0,0) , Quaternion.identity, Vector2.one);
            },
            () => {
                Debug.Log("Send Fail");
            });
    }
}
