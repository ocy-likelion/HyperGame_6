using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUIController : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _negativeButton;
    [SerializeField] private Button _pauseButton;
    
    private void Awake()
    {
        //버튼 클릭이벤트 등록
        _acceptButton.onClick.AddListener(OnClickAcceptButton);
        _negativeButton.onClick.AddListener(OnClickNegativeButton);
        _pauseButton.onClick.AddListener(OnClickPauseButton);
    }
    
    public void OnClickAcceptButton()
    {
        Classification.Instance.confirm = true; //승인버튼 클릭시 서류 승인
        Classification.Instance.DocumentClassification(); // 서류 분류 메소드 호출
        AudioManager.Instance.SFX.PlayStamp();
        
        //VFX 테스트 예시. 
        VfxManager.Instance.GetVFX(VFXType.TEST, new Vector2(0,0) , Quaternion.identity, Vector2.one);
    }

    public void OnClickNegativeButton()
    {
        Classification.Instance.confirm = false; //반려버튼 클릭시 서류 반려
        Classification.Instance.DocumentClassification(); // 서류 분류 메소드 호출
        AudioManager.Instance.SFX.PlayStamp();
    }
    
    public void OnClickPauseButton()
    {
        GameManager.Instance.PauseGame();
        UIManager.Instance.popupUIController.ShowPauseUI();
        AudioManager.Instance.SFX.PlayButtonClick();
    }
}
