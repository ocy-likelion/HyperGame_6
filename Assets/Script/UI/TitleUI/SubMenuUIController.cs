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

    private void Awake()
    {
        //버튼 클릭이벤트 등록
        _gameSettingButton.onClick.AddListener(OnClickGameSettingButton);
        _leaderBoardButton.onClick.AddListener(OnClickLeaderBoardButton);
    }
    
    public void OnClickGameSettingButton()
    {
        AudioManager.Instance.ToggleAudio();
    }

    public void OnClickLeaderBoardButton()
    {
        Debug.Log("OnClickLeaderBoardButton");
        AudioManager.Instance.SFX.PlayButtonClick();
    }
}
