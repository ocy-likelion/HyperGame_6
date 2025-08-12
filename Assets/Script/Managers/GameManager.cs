using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class GameManager : Singleton<GameManager>
{
    public InGameController inGameController;
    
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

    //일시정지 관리.
    private bool _isPaused;
    
    protected override void Initialize()
    {
        //Initialize
        _states[GameState.Title] = new TitleState();
        _states[GameState.InGame] = new InGameState();
        _states[GameState.Pause] = new PauseState();
        ChangeGameState(GameState.Title);
        
        inGameController = new InGameController();
        inGameController.Initialize();

        _isPaused = false;
    }
    
    private void Update()
    {
        if (_currentState != GameState.None)
        {
            _states[_currentState].OnUpdate();
        }
        
        //테스트용 입력
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GoToInGame();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ReturnToTitle();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ResumeGame();
        }
    }
    
    //게임을 시작합니다.
    public void GoToInGame()
    {
        ChangeGameState(GameState.InGame);
    }

    //타이틀로 돌아갑니다.
    public void ReturnToTitle()
    {
        ChangeGameState(GameState.Title);
    }

    ///게임을 일시정지 합니다.
    public void PauseGame()
    {
        if (_isPaused)
        {
            Debug.LogWarning("Game is already paused");
            return;
        }
        
        ChangeGameState(GameState.Pause);
        _isPaused = true;
    }

    //게임을 재개 합니다.
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

    //게임의 상태를 변경합니다.
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
    
    //일시정지(백그라운드 상태) 되었을 때
    private void OnApplicationPause(bool pauseStatus)
    {
        //Debug.Log("OnApplicationPause: " + pauseStatus);
    }

    //게임이 종료되었을 때
    private void OnApplicationQuit()
    {
        //Debug.Log("OnApplicationQuit");
    }

    public new void OnDestroy()
    {
        base.OnDestroy();
    }
}