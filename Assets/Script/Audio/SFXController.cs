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

    private AudioSource _sfxSource;
    private bool _isSFXOn = true;       // SFX가 켜져있는지 여부
    public bool IsSFXOn() => _isSFXOn;
    
    protected override void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _sfxSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
        AudioManager.Instance.SetSFXController(this);
    }

    // SFX를 추가하신 뒤, 아래 함수 모음에 재생 함수를 작성해주세요. 그리고 작성하신 함수를 통해 사용하시면 됩니다.
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
    public void PlayTimeOutAlert() => PlaySFX(timeOutAlert);
    public void PlayNewRecordResult() => PlaySFX(newRecordResult);
    public void PlayNewRecordScoreBar() => PlaySFX(newRecordScoreBar);
    #endregion

    // SFX 1번 재생
    private void PlaySFX(AudioClip clip)
    {
        if (!_isSFXOn || clip == null) return;
        _sfxSource.PlayOneShot(clip);
    }

    // _isSFXOn 조정
    public void SetSFXOn(bool isSFXOn)
    {
        _isSFXOn = isSFXOn;
    }
}