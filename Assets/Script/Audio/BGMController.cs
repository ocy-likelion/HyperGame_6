using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class BGMController : Singleton<BGMController>
{
    // BGM을 추가하실 때, 여기에 추가해주세요.
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private AudioClip gameBGM;
    
    // 여기까지
    
    private AudioSource _bgmSource;
    private bool _isBGMOn = true;       // BGM이 켜져있는지 여부
    public bool IsBGMOn() => _isBGMOn;

    protected override void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _bgmSource = gameObject.AddComponent<AudioSource>();
        DifficultyManager.OnLevelChanged += SetBGMSpeedFast;     // 난이도가 상승하면 자동 실행되도록 구독
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        AudioManager.Instance.SetBGMController(this);
        
        // TODO: 유저 정보에 소리 설정이 OFF라면 재생되지 않도록
    }

    // BGM를 추가하신 뒤, 아래 함수 모음에 재생 함수를 작성해주세요. 그리고 작성하신 함수를 통해 사용하시면 됩니다.
    #region PlayBGM 함수 모음 

    public void PlayTitleBGM() => PlayBGM(titleBGM);

    public void PlayGameBGM() => PlayBGM(gameBGM);

    #endregion

    // Scene에 따라 BGM 재생
    public void PlayBGMByState(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.Title:
                PlayTitleBGM();
                break;
            case GameState.InGame:
                PlayGameBGM();
                break;
            default:
                StopBGM();
                break;
        }
    }
    
    // BGM 재생 (반복 O)
    private void PlayBGM(AudioClip clip)
    {
        if (!_isBGMOn || clip == null) return;

        _bgmSource.clip = clip;
        _bgmSource.loop = true;
        _bgmSource.volume = 0.6f;
        _bgmSource.Play();
    }

    // BGM 중지
    private void StopBGM()
    {
        if (_bgmSource != null)
        {
            _bgmSource.Stop();
        }
    }

    // BGM 음소거
    private void MuteBGM()
    {
        if (_bgmSource != null)
        {
            _bgmSource.mute = true;
        }
    }

    // BGM 음소거 해제
    private void UnmuteBGM()
    {
        if (_bgmSource != null)
        {
            _bgmSource.mute = false;
        }
    }

    // BGM 볼륨을 절반으로 설정
    public void SetBGMVolumeHalf()
    {
        if (_bgmSource != null)
        {
            _bgmSource.volume = 0.3f;
        }
    }

    // BGM 볼륨을 기본으로 설정
    public void SetBGMVolumeMax()
    {
        if (_bgmSource != null)
        {
            _bgmSource.volume = 0.6f;
        }
    }

    // BGM 배속 조절
    private void SetBGMSpeedFast()
    {
        int level = DifficultyManager.Instance.GetLevel(GameManager.Instance.GetTimeController()._day);
        float[] bgmSpeeds = { 1f, 1.1f, 1.2f, 1.3f, 1.5f };
        int temp = Mathf.Clamp(level, 0, bgmSpeeds.Length - 1);     // level이 5 이상 넘어가는 것을 방지하기 위한 임시값
        
        _bgmSource.pitch = bgmSpeeds[temp];
    }

    // BGM 속도 정상화
    public void SetBGMSpeedNormal()
    {
        if (_bgmSource != null)
        {
            _bgmSource.pitch = 1f;
        }
    }

    // _isBGMOn값을 조정하고 그에 따라 BGM을 음소거 설정 및 해제
    public void SetBGMOn(bool isBGMOn)
    {
        _isBGMOn = isBGMOn;
        if (_isBGMOn)   // 음소거 해제
        {
            UnmuteBGM();
        }
        else   // 음소거
        {
            MuteBGM();
        }
    }
}