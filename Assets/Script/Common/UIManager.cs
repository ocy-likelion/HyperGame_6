using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] public TitleUIController titleUIController;
    [SerializeField] public InGameUIController inGameUIController;
    [SerializeField] public PopupUIController popupUIController;
    
    [SerializeField] private PanelController panelController;
    [SerializeField] private PopupController popupController;

    protected override void Initialize()
    {
        base.Initialize();
        
        // 초기화 로직 필요 시, 추가
    }

    public void SetTitleUIController(TitleUIController titleUIController)
    {
        this.titleUIController = titleUIController;
    }
    
    public void SetInGameUIController(InGameUIController inGameUIController)
    {
        this.inGameUIController = inGameUIController;
    }
    
    public void SetPopupUIController(PopupUIController popupUIController)
    {
        this.popupUIController = popupUIController;
    }
    
    public void ReleasePopupUIController()
    {
        popupUIController = null;
    }
    
    public void ShowBackgroundImage(bool isShow)
    {
        popupUIController.backgroundImage.gameObject.SetActive(isShow);
    }

    // Panel 관련
    public void OpenPanel(GameObject panel) => panelController.OpenPanel(panel);
    
    // Popup 관련
    public void ShowPopup(GameObject popup) => popupController.ShowPopup(popup);
    public void ClosePopup(GameObject popup) => popupController.ClosePopup(popup);
}