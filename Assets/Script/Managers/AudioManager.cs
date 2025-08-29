using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private BGMController bgmController;
    [SerializeField] private SFXController sfxController;
    
    // 각 Controller가 필요하시면 아래와 같이 호출해주세요.
    public BGMController BGM => bgmController;
    public SFXController SFX => sfxController;

    private bool _isAudioOn = true;     // Audio 토글이 On인지, Off인지

    protected override void Initialize()
    {
        base.Initialize();
        _isAudioOn = true;   // 기본값은 켜짐으로 설정
        
        // 필요 시, 초기화    ex) 저장된 설정 로드
    }
    
    public void SetBGMController(BGMController bgmController)
    {
        this.bgmController = bgmController;
    }
    
    public void SetSFXController(SFXController sfxController)
    {
        this.sfxController = sfxController;
    }

    // Audio 토글을 통해 BGM 및 SFX On/Off
    public void ToggleAudio()
    {
        _isAudioOn = !_isAudioOn;
        bgmController.SetBGMOn(_isAudioOn);
        sfxController.SetSFXOn(_isAudioOn);
    }

    public bool GetIsAudioOn()
    {
        return _isAudioOn;
    }
    
    //=> WebGL앱이 백그라운드로 가면 Runtime이 멈춰서 JS단계에서 멈추어 주어야함.
    //iOS는 음악까지 다 멈추지만 Android는 JS내에서 직접 음원중단 코드를 넣어야함.
    //그래도 iOS단계에서 안멈출까봐 일단 남겨둡니다.
    // 앱이 백그라운드로 갔을 때 Audio Mute
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)    // 백그라운드 --> 강제 음소거
        {
            bgmController.SetBGMOn(false);
            sfxController.SetSFXOn(false);
        }
        else                // 포그라운드 --> 유저 설정에 맞춰서 복원
        {
            bgmController.SetBGMOn(_isAudioOn);
            sfxController.SetSFXOn(_isAudioOn);
        }
    }
}