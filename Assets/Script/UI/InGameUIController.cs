using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class InGameUIController : Singleton<InGameUIController>
{
    [SerializeField] private RectTransform _inGameUI;
    public RectTransform activeUI;

    //새로 추가한 InGameUI는 이곳 아래에 추가해주시고, 프리팹의 자식개체로 넣은 뒤 인스펙터에서 할당해주세요.
    //이 싱글톤 객체를 통해 UI 접근을 용이하게 관리합니다.
    //ex. public TimerUIController timerUIController;
    public TimeUIController timeUIController;
    public InteractionUIController interactionUIController;
    public ScoreUIController scoreUIController;
    public ComboUIController comboUIController;
    public FeverUIController feverUIController;
    public BackgroundUIController backGroundUIController;
    public ClockUIController clockUIController;
    public ClassificationUIController classificationUIController;
    public WaitThreeSecondsUI waitThreeSecondsUI;
    public DifficultyUpEffectUIController difficultyUpEffectUIController;
    //여기까지

    protected override void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UIManager.Instance.SetInGameUIController(this);
        difficultyUpEffectUIController.Initialize();
    }

    public void SetSafeAreaOffset()
    {
        var offset = new Vector2(0, GameManager.Instance.cachedSafeAreaValue);
        var pauseBtnRect = interactionUIController.GetPauseButton().transform as RectTransform;
        activeUI.anchoredPosition -= offset;
        if (pauseBtnRect != null) pauseBtnRect.anchoredPosition -= offset;
    }

    public void ShowInGameUI()
    {
        _inGameUI.gameObject.SetActive(true);
    }

    public void HideInGameUI()
    {
        _inGameUI.gameObject.SetActive(false);
    }

    #region TimeUI
    public void ShowTimeUI()
    {
        timeUIController.gameObject.SetActive(true);
    }

    public void HideTimeUI()
    {
        timeUIController.gameObject.SetActive(false);
    }
    #endregion

    #region InteractionUI
    public void ShowInteractionUI()
    {
        interactionUIController.gameObject.SetActive(true);
    }

    public void HideInteractionUI()
    {
        interactionUIController.gameObject.SetActive(false);
    }
    #endregion

    #region ScoreUI
    public void ShowScoreUI()
    {
        scoreUIController.gameObject.SetActive(true);
    }

    public void HideScoreUI()
    {
        scoreUIController.gameObject.SetActive(false);
    }
    #endregion

    #region FeverUI
    public void ShowFeverUI()
    {
        feverUIController.gameObject.SetActive(true);
    }

    public void HideFeverUI()
    {
        feverUIController.gameObject.SetActive(false);
    }
    #endregion

    #region BackgroundUI
    public void ShowBackgroundUI()
    {
        backGroundUIController.gameObject.SetActive(true);
    }

    public void HideBackgroundUI()
    {
        backGroundUIController.gameObject.SetActive(false);
    }
    #endregion

    #region ClockUI
    public void ShowClockUI()
    {
        clockUIController.gameObject.SetActive(true);
    }
    public void HideClockUI()
    {
        clockUIController.gameObject.SetActive(false);
    }
    #endregion

    #region ClassificationUI
    public void ShowClassificationUI()
    {
        classificationUIController.gameObject.SetActive(true);
    }
    public void HideClassificationUI()
    {
        classificationUIController.gameObject.SetActive(false);
    }
    #endregion

    #region WaitThreeSecondsUI
    public void ShowWaitThreeSecondsUI()
    {
        waitThreeSecondsUI.gameObject.SetActive(true);
    }
    public void HideWaitThreeSecondsUI()
    {
        waitThreeSecondsUI.gameObject.SetActive(false);
    }
    #endregion
    
    #region DifficultyUpEffectUI

    public void CallDifficultyUpEffect()
    {
        StartCoroutine(difficultyUpEffectUIController.ShowEffectUI());
    }
    public void ShowDifficultyUpEffectUI()
    {
        difficultyUpEffectUIController.gameObject.SetActive(true);
    }
    public void HideDifficultyUpEffectUI()
    {
        difficultyUpEffectUIController.gameObject.SetActive(false);
    }
    #endregion
}