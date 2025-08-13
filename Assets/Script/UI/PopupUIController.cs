using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupUIController : Singleton<PopupUIController>
{
    [SerializeField] public Image backgroundImage;
    
    //새로 추가한 InGameUI는 이곳 아래에 추가해주시고, 프리팹의 자식개체로 넣은 뒤 인스펙터에서 할당해주세요.
    //이 싱글톤 객체를 통해 UI 접근을 용이하게 관리합니다.
    //ex. public PauseUIController pauseUIController;
    public PauseUIController pauseUIController;
    
    
    //여기까지
    
    protected override void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UIManager.Instance.SetPopupUIController(this);
    }

    #region PauseUI
    public void ShowPauseUI()
    {
        pauseUIController.ShowPopup();
    }
    
    public void HidePauseUI()
    {
        pauseUIController.ClosePopup();
    }
    #endregion
    
}
