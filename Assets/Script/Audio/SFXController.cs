using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXController : Singleton<SFXController>
{
    // SFX를 추가하실 때 여기에 추가해주세요.
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip stamp;
    
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