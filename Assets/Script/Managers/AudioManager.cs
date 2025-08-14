using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private BGMController bgmController;
    [SerializeField] private SFXController sfxController;
    
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
}