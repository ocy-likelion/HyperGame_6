using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PopupUIController : Singleton<PopupUIController>
{
    [SerializeField] public Image backgroundImage;
    [SerializeField] public Image errorReportBgImage;
    [SerializeField] public Image adBackgroundImage;
    [SerializeField] public TMP_Text AdmobDebugText;
    
    //새로 추가한 InGameUI는 이곳 아래에 추가해주시고, 프리팹의 자식개체로 넣은 뒤 인스펙터에서 할당해주세요.
    //이 싱글톤 객체를 통해 UI 접근을 용이하게 관리합니다.
    //ex. public PauseUIController pauseUIController;
    public PauseUIController pauseUIController;
    public ResultUIController resultUIController;
    public GameOverUIController gameOverUIController;
    public TutorialUIControllerRE tutorialUIController;
    public InfoUIController InfoUIController;
    public IntroUIController introUIController;
    public ErrorReportUIController errorReportUIController;
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
    
    public void SetAdmobDebugText(string text)
    {
        AdmobDebugText.text = text;
    }

    public void ShowAdBg()
    {
        adBackgroundImage.gameObject.SetActive(true);
    }

    public void HideAdBg()
    {
        adBackgroundImage.gameObject.SetActive(false);
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

    #region ResultUI
    public void ShowResultUI()
    {
        resultUIController.ShowPopup();
    }
    public void HideResultUI()
    {
        resultUIController.ClosePopup();
    }
    #endregion
    
    #region GameOverUI
    public void ShowGameOverUI()
    {
        gameOverUIController.ShowPopup();
    }
    public void HideGameOverUI()
    {
        gameOverUIController.ClosePopup();
    }
    #endregion

    #region TutorialUI
    public void ShowTutorialUI()
    {
        tutorialUIController.ShowPopup();
    }

    public void HideTutorialUI()
    {
        tutorialUIController.ClosePopup();
    }
    #endregion

    #region InfoUI
    public void ShowInfoUI()
    {
        InfoUIController.ShowPopup();
    }

    public void HideInfoUI()
    {
        InfoUIController.ClosePopup();
    }
    #endregion
    
    #region IntroUI
    public void ShowIntroUI()
    {
        introUIController.ShowPopup();
    }

    public void HideIntroUI()
    {
        introUIController.ClosePopup();
    }
    #endregion
    
    #region ErrorReportUI
    
    public void ShowErrorReportUI()
    {
        if(errorReportUIController != null)
            errorReportUIController.ShowPopup();
    }

    public void HideErrorReportUI()
    {
        if(errorReportUIController != null)
            errorReportUIController.ClosePopup();
    }
    #endregion
}
