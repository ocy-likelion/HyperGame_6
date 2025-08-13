using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TitleUIController : Singleton<TitleUIController>
{
    [SerializeField] private RectTransform _titleUI;
    
    //새로 추가한 TitleUI는 이곳 아래에 추가해주시고, 프리팹의 자식개체로 넣은 뒤 인스펙터에서 할당해주세요.
    //이 싱글톤 객체를 통해 UI 접근을 용이하게 관리합니다.
    //ex. public TimerUIController timerUIController;
    public MainMenuUIController mainMenuUIController;
    public SubMenuUIController subMenuUIController;
    
    //여기까지
    
    protected override void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UIManager.Instance.SetTitleUIController(this);
    }
    
    public void ShowTitleUI()
    {
        _titleUI.gameObject.SetActive(true);
    }

    public void HideTitleUI()
    {
        _titleUI.gameObject.SetActive(false);
    }
    
    #region MainMenuUI
    public void ShowMainMenuUI()
    {
        mainMenuUIController.gameObject.SetActive(true);
    }

    public void HideMainMenuUI()
    {
        mainMenuUIController.gameObject.SetActive(false);
    }
    #endregion

    #region SubMenuUI
    public void ShowSubMenuUI()
    {
        subMenuUIController.gameObject.SetActive(true);
    }

    public void HideSubMenuUI()
    {
        subMenuUIController.gameObject.SetActive(false);
    }
    #endregion
    
}
