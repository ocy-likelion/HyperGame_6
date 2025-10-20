//#define DEBUG //디버깅 사용시에만 활성화 할것

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using static Constants;

public class GameManager : Singleton<GameManager>
{
    public InGameController inGameController;
    public ObstacleClearEffect obstacleClearEffect;
    
    /// <summary>
    /// 게임은 상태패턴으로 관리됩니다. Title, InGame, Pause 세가지로 관리됩니다.
    /// IGameState 인터페이스를 기반으로 만들어졌으며, 각 상태에 진입(OnEnter)할 때 씬을 로드합니다.
    /// GameState는 ChangeGameState를 통해 변경합니다.
    /// </summary>
    private GameState _previousState;
    private GameState _currentState;
    //public GameState CurrentGameState => _currentState;
    private Dictionary<GameState, IGameState> _states = new Dictionary<GameState, IGameState>();
    public Action<GameState> GameStateChanged;
    
    //토스앱 버전 체크
    [FormerlySerializedAs("isSupported")] public bool isSupportedCheck = false;
    
    //일시정지 관리.
    public bool _isPaused;
    
    //파티클 로드 여부
    public bool particleLoadOn;
    public static IEnumerator warmUpParticleLoad;
    
    protected override void Initialize()
    {
        //Initialize
        _states[GameState.Title] = new TitleState();
        _states[GameState.InGame] = new InGameState();
        _states[GameState.Pause] = new PauseState();
        
        inGameController = new InGameController();
        obstacleClearEffect = new ObstacleClearEffect();
        obstacleClearEffect.Initialize();
        
        _isPaused = false;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _= LoadGameSystem();
    }

    //최초 실행시 초기화 진행
    private async Task LoadGameSystem()
    {
        //셰이더 선로드
        Shader.WarmupAllShaders();
        while (warmUpParticleLoad == null)
        {
            await Task.Yield();
        }
        StartCoroutine(warmUpParticleLoad);
        
        //인트로 초기화
        while (UIManager.Instance.popupUIController.introUIController == null)
        {
            await Task.Yield();
        }
        UIManager.Instance.popupUIController.introUIController.InitUI();
        
        //Addressable 데이터 로드
        await obstacleClearEffect.LoadSprites();
        await UIManager.Instance.popupUIController.pauseUIController.LoadSprites();
        await UIManager.Instance.titleUIController.subMenuUIController.LoadSprites();
        await UIManager.Instance.inGameUIController.interactionUIController.LoadSprites();
        
        NetworkManager.Instance.CheckAppVersion();//토스앱 버전 체크
        
        //버전 체크가 완료 될때까지 대기
        while (!isSupportedCheck)
        {
            await Task.Yield();
        }
        
        StartCoroutine(LoadEndInitGame());
    }

    private IEnumerator LoadEndInitGame()
    {
        yield return new WaitUntil(() => particleLoadOn);
        yield return StartCoroutine(
            UIManager.Instance.popupUIController.introUIController.InitIntroUI());
        yield return StartCoroutine(inGameController.Initialize());
        ChangeGameState(GameState.Title);
        yield return null;
    }
    
    private void Update()
    {
        if (_currentState != GameState.None)
        {
            _states[_currentState].OnUpdate();
        }
        
#if DEBUG
        DebugInputs();//디버그용 입력 체크
#endif
    }
    
    ///게임을 시작합니다.
    public void GoToInGame()
    {
        ChangeGameState(GameState.InGame);
    }

    ///타이틀로 돌아갑니다.
    public void ReturnToTitle()
    {
        ChangeGameState(GameState.Title);
    }

    ///게임을 일시정지 합니다.
    public void PauseGame()
    {
        GetDocumentController()._isClickable = false;
        
        if (_isPaused)
        {
            Debug.LogWarning("Game is already paused");
            return;
        }
        
        ChangeGameState(GameState.Pause);
        _isPaused = true;
    }

    ///게임을 재개 합니다.
    public void ResumeGame()
    {
        if (!_isPaused)
        {
            Debug.LogWarning("Game is not paused");
            return;
        }
        
        ChangeGameState(GameState.Pause, true);
        _isPaused = false;
    }

    ///게임의 상태를 변경합니다.
    public void ChangeGameState(GameState newGameState, bool resume = false)
    {
        //기존 State 종료
        if (_currentState != GameState.None)
        {
            _states[_currentState].OnExit();
        }

        //일시정지 해제 시
        if (_currentState == GameState.Pause && newGameState == GameState.Pause && resume)
        {
            _currentState = _previousState;
        }
        else//새 State로 전환
        {
            _previousState = _currentState;
            _currentState = newGameState;
            
            _states[_currentState].OnEnter();
        }
        
        //State전환 후 실행할 Action이 있으면 실행
        GameStateChanged?.Invoke(_currentState);
    }
    
    ///TimeController가 필요할땐 이 함수를 쓰시면 됩니다.
    public TimeController GetTimeController()
    {
        return inGameController.timeController != null ? inGameController.timeController : null;
    }
    
    ///DocumentController가 필요할땐 이 함수를 쓰시면 됩니다.
    public DocumentController GetDocumentController()
    {
        return inGameController.docController != null ? inGameController.docController : null;
    }

    ///Classification이 필요할땐 이 함수를 쓰시면 됩니다.
    public Classification GetClassification()
    {
        return inGameController.classification != null ? inGameController.classification : null;
    }

    public GameState GetGameState()
    {
        return _currentState;
    }

    ///일시정지(백그라운드 상태) 되었을 때
    private void OnApplicationPause(bool pauseStatus)
    {
        //Debug.Log("OnApplicationPause: " + pauseStatus);
    }

    ///게임이 종료되었을 때
    private void OnApplicationQuit()
    {
        //Debug.Log("OnApplicationQuit");
    }

    public new void OnDestroy()
    {
        obstacleClearEffect.ClearSprites();//List에 로드한 스프라이트 해제
        base.OnDestroy();
    }

    private void DebugInputs()
    {
        // //테스트용 입력
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //NetworkManager.Instance.OnScoreEvent("FailedToSend");
            NetworkManager.Instance.OnScoreEvent("VersionNotSupported");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NetworkManager.Instance.OnAdEvent("failedToShow");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            NetworkManager.Instance.OnAdEvent("TestErrMsg");
        }
        
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     GoToInGame();
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     //게임오버 시키기
        //     inGameController.QuitGame();
        //     //ReturnToTitle();
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     PauseGame();
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha4))
        // {
        //     ResumeGame();
        // }
    }
}