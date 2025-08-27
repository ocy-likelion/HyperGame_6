using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private Sprite[] _acceptBtnSprites = new Sprite[2];
    private Sprite[] _negaviteBtnSprites = new Sprite[2];
    
    private void Awake()
    {
        //버튼 클릭이벤트 등록
        _acceptButton.onClick.AddListener(OnClickAcceptButton);
        _negativeButton.onClick.AddListener(OnClickNegativeButton);
        _pauseButton.onClick.AddListener(OnClickPauseButton);
    }
    
    public async Task LoadSprites()
    {
        _acceptBtnSprites[0] = await DataManager.Instance.LoadSpriteData(Addresses.Sprites.Buttons.Interactions.ApproveUp);
        _acceptBtnSprites[1] = await DataManager.Instance.LoadSpriteData(Addresses.Sprites.Buttons.Interactions.ApproveDown);
        _negaviteBtnSprites[0] = await DataManager.Instance.LoadSpriteData(Addresses.Sprites.Buttons.Interactions.NotApproveUp);
        _negaviteBtnSprites[1] = await DataManager.Instance.LoadSpriteData(Addresses.Sprites.Buttons.Interactions.NotApproveDown);
    }

    public IEnumerator FeverMode()
    {
        //스프라이트 변경
        _negativeButton.image.sprite = _acceptBtnSprites[0];
        var state = _negativeButton.spriteState;
        state.pressedSprite = _acceptBtnSprites[1];
        _negativeButton.spriteState = state;
        
        //피버가 true인동안 유지
        while (GameManager.Instance.GetClassification().fever)
        {
            yield return null;
        }
        
        //원복
        _negativeButton.image.sprite = _negaviteBtnSprites[0];
        state.pressedSprite = _negaviteBtnSprites[1];
        _negativeButton.spriteState = state;
    }
    
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

        //피버일때는 둘다 통과처리
        if (GameManager.Instance.GetClassification().fever)
        {
            GameManager.Instance.GetClassification().confirm = true; //반려버튼 클릭시 서류 반려
            GameManager.Instance.GetDocumentController().ShowStamp(true);
        }
        else
        {
            GameManager.Instance.GetClassification().confirm = false; //반려버튼 클릭시 서류 반려
            GameManager.Instance.GetDocumentController().ShowStamp(false);
        }
        AudioManager.Instance.SFX.PlayStamp();
        GameManager.Instance.GetClassification().DocumentClassification(); // 서류 분류 메소드 호출
    }
    
    public void OnClickPauseButton()
    {
        GameManager.Instance.PauseGame();
        UIManager.Instance.popupUIController.ShowPauseUI();
        AudioManager.Instance.SFX.PlayButtonClick();
    }
}
