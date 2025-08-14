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
    private AudioClip _currentBGM;     // 현재 재생해야 할 BGM 저장
    private bool _isBGMOn = true;       // BGM이 켜져있는지 여부
    public bool IsBGMOn() => _isBGMOn;

    protected override void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _bgmSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
        AudioManager.Instance.SetBGMController(this);
        
        // Scene 이름에 맞게 BGM 재생
        // TODO: 유저 정보에 소리 설정이 OFF라면 재생되지 않도록
        switch (scene.name)
        {
            case TitleSceneName:
                PlayTitleBGM();
                break;
            case GameSceneName:
                PlayGameBGM();
                break;
            default:
                StopBGM();
                break;
        }
    }

    // BGM를 추가하신 뒤, 아래 함수 모음에 재생 함수를 작성해주세요. 그리고 작성하신 함수를 통해 사용하시면 됩니다.
    #region PlayBGM 함수 모음 

    public void PlayTitleBGM() => PlayBGM(titleBGM);

    public void PlayGameBGM() => PlayBGM(gameBGM);

    #endregion

    // BGM 재생 (반복 O)
    private void PlayBGM(AudioClip clip)
    {
        _currentBGM = clip;     // 현재 BGM 저장
        if (!_isBGMOn || clip == null) return;

        _bgmSource.clip = clip;
        _bgmSource.loop = true;
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

    // _isBGMOn값을 조정하고 그에 따라 BGM 재생 및 중지
    public void SetBGMOn(bool isBGMOn)
    {
        _isBGMOn = isBGMOn;
        if (_isBGMOn && _currentBGM != null)   // 재생
        {
            PlayBGM(_currentBGM);
        }
        else   // 중지
        {
            StopBGM();
        }
    }
}