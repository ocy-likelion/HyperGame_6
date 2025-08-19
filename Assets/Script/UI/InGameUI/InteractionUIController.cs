using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractionUIController : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _negativeButton;
    [SerializeField] private Button _pauseButton;
    // [SerializeField] private Sprite[] _buttonSprites;
    //
    // private Sprite _acceptUpSprite;
    // private Sprite _acceptDownSprite;
    // private Sprite _negativeUpSprite;
    // private Sprite _negativeDownSprite;
    // private Sprite _pauseUpSprite;
    // private Sprite _pauseDownSprite;
    
    private void Awake()
    {
        //버튼 클릭이벤트 등록
        _acceptButton.onClick.AddListener(OnClickAcceptButton);
        // _acceptUpSprite = FindSprite(Constants.SprAcceptUp);
        //_acceptDownSprite = FindSprite(Constants.SprAcceptDown);
        
        _negativeButton.onClick.AddListener(OnClickNegativeButton);
        // _negativeUpSprite = FindSprite(Constants.SprNegativeUp);
        // _negativeDownSprite = FindSprite(Constants.SprNegativeDown);;
        
        _pauseButton.onClick.AddListener(OnClickPauseButton);
        // _pauseUpSprite = FindSprite(Constants.SprPauseUp);
        // _pauseDownSprite = FindSprite(Constants.SprPauseDown);
    }
    
    // public Sprite FindSprite(string spriteName)
    // {
    //     return _buttonSprites.FirstOrDefault(sprite => sprite.name == spriteName);
    // }
    
    public void OnClickAcceptButton()
    {
        //서류 처리가능이 true일때만 진행
        if (!GameManager.Instance.GetDocumentController()._isClickable) return;
        
        GameManager.Instance.GetClassification().confirm = true; //승인버튼 클릭시 서류 승인
        AudioManager.Instance.SFX.PlayStamp();
        GameManager.Instance.GetDocumentController().ShowStamp(true);
        GameManager.Instance.GetClassification().DocumentClassification(); // 서류 분류 메소드 호출
        
        //VFX 테스트 예시. 
        //VfxManager.Instance.GetVFX(VFXType.TEST, new Vector2(0,0) , Quaternion.identity, Vector2.one);
    }

    public void OnClickNegativeButton()
    {
        //서류 처리가능이 true일때만 진행
        if (!GameManager.Instance.GetDocumentController()._isClickable) return;
        
        GameManager.Instance.GetClassification().confirm = false; //반려버튼 클릭시 서류 반려
        AudioManager.Instance.SFX.PlayStamp();
        GameManager.Instance.GetDocumentController().ShowStamp(false);
        GameManager.Instance.GetClassification().DocumentClassification(); // 서류 분류 메소드 호출
    }
    
    public void OnClickPauseButton()
    {
        GameManager.Instance.PauseGame();
        UIManager.Instance.popupUIController.ShowPauseUI();
        AudioManager.Instance.SFX.PlayButtonClick();
    }
}
