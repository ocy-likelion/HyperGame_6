using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

//인게임 주요 로직들을 제어합니다.
public class InGameController
{
    public TimeController timeController;
    public DocumentController docController;
    public Classification classification;
    
    public bool Initialized;
    
    private bool _initComplete;
    private bool _gameStarted;
    private bool _gameFinished;
    private bool _quitGame;
    private bool _skipResultUI;
    private bool _useRetry;
    private bool _newRecordOn;
    private bool _endAdmob;
    
    
    public IEnumerator Initialize()
    {
        //TODO: 게임 실행시 초기화 할 로직
        //classification = new Classification();
        
        //씬 내 타이머, 문서생성 오브젝트 찾기
        if (timeController == null)
        {
            yield return new WaitUntil(() => timeController = Object.FindObjectOfType<TimeController>());
        }
        if (docController == null)
        {
            yield return new WaitUntil(() => docController = Object.FindObjectOfType<DocumentController>());
        }
        if(classification == null)
        {
            yield return new WaitUntil(() => classification = Object.FindObjectOfType<Classification>());
            classification.Initialize();
        }
        
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
        //실행 필수 초기화 완료전 까지 대기
        yield return new WaitUntil(() => Initialized);
        
        //TODO: 게임 시작시 초기화 할 로직
        _initComplete = false;
        
        _gameStarted = false;
        _gameFinished = false;
        _quitGame = false;
        _skipResultUI = false;
        _useRetry = false;
        _newRecordOn = false;
        _endAdmob = false;
        
        // BGM 초기화
        AudioManager.Instance.BGM.SetBGMVolumeMax();    // 볼륨을 최대로
        AudioManager.Instance.BGM.SetBGMSpeedNormal();  // 배속을 기본으로

        //타이머 초기화
        timeController.InitTimeController();
        
        //서류 풀 초기화
        docController.ReloadDocument(true);
        
        //classification.InitScore();
        GameManager.Instance.GetClassification().InitScore();
        _initComplete = true;
        
        // NewRecord 이미지 위치 초기화
        InGameUIController.Instance.scoreUIController.InitNewRecordImage();
        
        // 시계 프레임 색상 초기화
        InGameUIController.Instance.clockUIController.InitClockFrameColor();
    }

    
    //게임 시작
    public IEnumerator StartGame()
    {
        _gameStarted = true;
        //인게임UI 보이기
        UIManager.Instance.inGameUIController.ShowTimeUI();
        UIManager.Instance.inGameUIController.ShowInteractionUI();
        UIManager.Instance.inGameUIController.ShowScoreUI();
        UIManager.Instance.inGameUIController.ShowFeverUI();
        UIManager.Instance.inGameUIController.ShowBackgroundUI();
        UIManager.Instance.inGameUIController.ShowClockUI();
        UIManager.Instance.inGameUIController.ShowClassificationUI();
        UIManager.Instance.inGameUIController.ShowWaitThreeSecondsUI();
        UIManager.Instance.inGameUIController.ShowDifficultyUpEffectUI();
        
        // 인게임 BGM 재생
        AudioManager.Instance.BGM.PlayBGMByState(GameManager.Instance.GetGameState());
        
        //321
        yield return UIManager.Instance.StartCoroutine(
            UIManager.Instance.inGameUIController.waitThreeSecondsUI.WaitThreeSeconds()
        );

        //난이도 상승시점 모니터링 시작
        DifficultyManager.Instance.InitLevelMonitor();

        //타이머, 문서 생성 시작.
        timeController.StartRunningTimer();
        docController.InitDocuments();
        
        while (!_gameFinished)
        {
            yield return null;
        }
    }

    //게임 종료. 결과 보고
    public IEnumerator EndGame()
    {
        //TODO: 게임 끝낼 시 실행할 로직
        
        //시간 정지
        timeController.StopTime();
        
        // 시간 부족 SFX 반복 재생 중지
        AudioManager.Instance.SFX.StopTimeOutAlert();
        
        //ex.게임오버 연출, 결과창UI등
        
        var popupController = UIManager.Instance.popupUIController;

        if (!_useRetry)//재시작 활성화 시 엔드연출 스킵
        {
            //게임오버
            var gameOverUI = popupController.gameOverUIController;
            yield return gameOverUI.ShowSequence(); //게임오버 연출동안 딜레이
        }
        
        
        if (!_skipResultUI)//필요 시 스킵
        {
            //결과창
            var resultUI = popupController.resultUIController;
            popupController.ShowResultUI();
            resultUI.InitResultItem(new GameResultData(
                timeController._day,
                GameManager.Instance.GetClassification().GetMaxCombo(),
                GameManager.Instance.GetClassification().GetScore()));
        }
        
        //게임이 끝난 후, 바로 돌아가지 않고 대기.
        while (!_quitGame)
        {
            yield return null;
        }
        
        //결과창이 떠야 게임 한판을 완료했다는 것이므로
        if (!_skipResultUI)
        {
            //광고호출
            NetworkManager.Instance.ShowAd();

            //광고 끝나기 전까지 대기
            while (!_endAdmob)
            {
                yield return null;
            }
        
            //광고 제어 변수 초기화
            _endAdmob = false;
        
            //다음 광고 로드
            NetworkManager.Instance.LoadAd();
        }  
        
        //초기화.
        _initComplete = false;
        _skipResultUI = false;
        
        //재시작 필요 시.
        if (_useRetry)
        {
            //새 게임 코루틴 활성화
            yield return GameManager.Instance.StartCoroutine(SetInitGame());
            GameManager.Instance.StartCoroutine(RunSequence());
            
            //재시작을 위해 타이틀로 복귀하지 않고 기존 코루틴을 중단한다.
            yield break;
        }
        
        // BGM 초기화
        AudioManager.Instance.BGM.SetBGMVolumeMax();    // 볼륨을 최대로
        AudioManager.Instance.BGM.SetBGMSpeedNormal();  // 배속을 기본으로
        
        //인게임 UI 닫기
        UIManager.Instance.inGameUIController.HideInGameUI();
        UIManager.Instance.inGameUIController.HideTimeUI();
        UIManager.Instance.inGameUIController.HideInteractionUI();
        UIManager.Instance.inGameUIController.HideScoreUI();
        UIManager.Instance.inGameUIController.HideFeverUI();
        UIManager.Instance.inGameUIController.HideBackgroundUI();
        UIManager.Instance.inGameUIController.HideClockUI();
        UIManager.Instance.inGameUIController.HideClassificationUI();
        UIManager.Instance.inGameUIController.HideWaitThreeSecondsUI();
        UIManager.Instance.inGameUIController.HideDifficultyUpEffectUI();
        
        //타이틀 씬으로 복귀
        GameManager.Instance.ReturnToTitle();
    }
    
    // New Record 체크 및 연출 재생
    public void CheckNewRecord(float currentScore)
    {
        if (_newRecordOn) return;   // 이미 NewRecord 연출이 재생됐으면 넘기도록

        float bestScore = PlayerPrefs.GetFloat("BestScore", 0f);
        if (currentScore > bestScore)
        {
            // New Record 연출
            InGameUIController.Instance.scoreUIController.ShowNewRecordImage();
            AudioManager.Instance.SFX.PlayNewRecordScoreBar();

            _newRecordOn = true;
        }
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

    ///결과 UI 스킵필요 시 게임 종료 전 호출
    public void SkipResultUI()
    {
        _skipResultUI = true;
    }

    //재시작 필요 시 먼저 호출.
    public void UseRetry()
    {
        _useRetry = true;
    }

    public bool GetGameStarted()
    {
        return _gameStarted;
    }

    public void EndAdMob()
    {
        _endAdmob = true;
    }
}
