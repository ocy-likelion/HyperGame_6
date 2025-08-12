using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인게임 주요 로직들을 제어합니다.
public class InGameController
{
    public bool Initialized;
    
    private bool _initComplete;
    private bool _gameStarted;
    private bool _gameFinished;
    private bool _quitGame;

    public void Initialize()
    {
        //TODO: 게임 실행시 초기화 할 로직
        Initialized = true;
    }
    
    //게임이 시작되면 이 시퀀스를 통해 진행됩니다.
    public IEnumerator RunSequence()
    {
        //실행 필수 초기화 진행 체크
        yield return new WaitUntil(() => Initialized);
        
        //게임 시퀀스 실행
        yield return new WaitUntil(() => _initComplete);
        yield return StartGame();
        yield return new WaitUntil(() => _gameFinished);
        yield return EndGame();
    }
    
    //게임 시작 전 초기화
    public IEnumerator SetInitGame()
    {
        //TODO: 게임 시작시 초기화 할 로직
        _gameStarted = false;
        _gameFinished = false;
        _quitGame = false;
        
        yield return null;
        
        _initComplete = true;
    }

    
    //게임 시작
    public IEnumerator StartGame()
    {
        _gameStarted = true;
        //TODO: 게임시작 후 실행할 로직
        
        //ex. workTime, Day등 시작
        
        //게임 끝내기 전까지 시퀀스 유지
        while (!_gameFinished)
        {
            yield return null;
        }
    }

    //게임 종료. 결과 보고
    public IEnumerator EndGame()
    {
        //TODO: 게임 끝낼 시 실행할 로직
        Debug.Log("Game Ended");
        //ex.게임오버 연출, 결과창UI등
        
        //게임이 끝난 후, 바로 돌아가지 않고 대기.
        while (!_quitGame)
        {
            yield return null;
        }

        //타이틀로 돌아감
        _initComplete = false;
        GameManager.Instance.ReturnToTitle();
    }

    ///게임 끝내기, 호출 시 진행중인 게임이 끝납니다.
    public void Dispose()
    {
        //게임이 시작했을때만
        if(_gameStarted) _gameFinished = true;
    }

    ///게임 끝난 후, 호출 시 타이틀로 돌아갑니다.
    public void QuitGame()
    {
        if(_gameFinished) _quitGame = true;
    }
}
