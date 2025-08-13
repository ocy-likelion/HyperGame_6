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
        
    }

    public void OnClickNegativeButton()
    {

    }
    
    public void OnClickPauseButton()
    {
        GameManager.Instance.PauseGame();
        UIManager.Instance.popupUIController.ShowPauseUI();
    }
}
