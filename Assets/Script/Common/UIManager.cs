using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private PanelController panelController;
    [SerializeField] private PopupController popupController;

    protected override void Initialize()
    {
        base.Initialize();
        
        // 초기화 로직 필요 시, 추가
    }

    // Panel 관련
    public void OpenPanel(GameObject panel) => panelController.OpenPanel(panel);
    
    // Popup 관련
    public void ShowPopup(GameObject popup) => popupController.ShowPopup(popup);
    public void ClosePopup(GameObject popup) => popupController.ClosePopup(popup);
}