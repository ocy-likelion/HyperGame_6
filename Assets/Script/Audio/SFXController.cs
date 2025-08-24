using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXController : Singleton<SFXController>
{
    // SFX를 추가하실 때 여기에 추가해주세요.
    [Header("Common")]
    [SerializeField] private AudioClip buttonClick;
    
    [Header("InGame")]
    [SerializeField] private AudioClip stamp;
    [SerializeField] private AudioClip docSuccess;
    [SerializeField] private AudioClip docFail;
    [SerializeField] private AudioClip docSwap;
    [SerializeField] private AudioClip obsBugPostHit;
    [SerializeField] private AudioClip obsProcessTry;
    [SerializeField] private AudioClip obsHandHit;
    [SerializeField] private AudioClip obsFileEnvelopeOut;
    [SerializeField] private AudioClip newRecordResult;
    [SerializeField] private AudioClip newRecordScoreBar;
    [SerializeField] private AudioClip speedUp;
    [SerializeField] private AudioClip fever;
    [SerializeField] private AudioClip timeOutAlert;
    
    
    // 여기까지

    private AudioSource _sfxSource;                             // 단발성 AudioSource
    private Dictionary<AudioClip, AudioSource> _loopSources;    // 반복용 AudioSource
    private bool _isSFXOn = true;       // SFX가 켜져있는지 여부
    public bool GetIsSFXOn() => _isSFXOn;
    
    protected override void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // AudioSource 초기화
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.playOnAwake = false;
        _loopSources = new Dictionary<AudioClip, AudioSource>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        AudioManager.Instance.SetSFXController(this);
    }

    // SFX를 추가하신 뒤, 아래 함수 모음에 재생 함수를 작성해주세요. 그리고 작성하신 함수를 통해 사용하시면 됩니다.
    // 1번 재생 : PlaySFX()
    // 반복 재생 : PlayLoopSFX()
    // 반복 재생 중지 : StopLoopSFX()
    #region PlaySFX 함수 모음

    public void PlayButtonClick() => PlaySFX(buttonClick);

    public void PlayStamp() => PlaySFX(stamp);
    
    public void PlayDocSuccess() => PlaySFX(docSuccess);
    public void PlayDocFail() => PlaySFX(docFail);
    public void PlayDocSwap() => PlaySFX(docSwap);
    public void PlayObsBugPostHit() => PlaySFX(obsBugPostHit);
    public void PlayObsProcessTry() => PlaySFX(obsProcessTry);
    public void PlayObsHandHit() => PlaySFX(obsHandHit);
    public void PlayObsFileEnvelopeOut() => PlaySFX(obsFileEnvelopeOut);
    public void PlaySpeedUp() => PlaySFX(speedUp);
    public void PlayFever() => PlaySFX(fever);
    public void PlayTimeOutAlert() => PlayLoopSFX(timeOutAlert);
    public void StopTimeOutAlert() => StopLoopSFX(timeOutAlert);
    public void PlayNewRecordResult() => PlaySFX(newRecordResult);
    public void PlayNewRecordScoreBar() => PlaySFX(newRecordScoreBar);
    #endregion

    // SFX 1번 재생
    private void PlaySFX(AudioClip clip)
    {
        if (!_isSFXOn || clip == null) return;
        _sfxSource.PlayOneShot(clip);
    }
    
    // SFX 반복 재생
    private void PlayLoopSFX(AudioClip clip)
    {
        if (!_isSFXOn || clip == null) return;
        if (_loopSources.ContainsKey(clip)) return;     // 이미 재생 중이면 패스
        
        var src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = true;
        src.clip = clip;
        src.Play();
        _loopSources[clip] = src;
    }
    
    // SFX 반복 재생 중지
    private void StopLoopSFX(AudioClip clip)
    {
        if (clip == null || !_loopSources.ContainsKey(clip)) return;

        var src = _loopSources[clip];
        if (src != null && src.isPlaying)
        {
            src.Stop();
            Destroy(src);
        }
        _loopSources.Remove(clip);
    }

    // _isSFXOn 조정
    public void SetSFXOn(bool isSFXOn)
    {
        _isSFXOn = isSFXOn;

        if (!_isSFXOn)  // 모든 SFX 정지
        {
            _sfxSource.Stop();
            
            foreach (var key in _loopSources)
                if (key.Value != null) key.Value.Stop();
            
            _loopSources.Clear();
        }
    }
}