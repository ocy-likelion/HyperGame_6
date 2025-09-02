using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoUIController : PopupController
{
    [SerializeField] private TMP_Text versionText;
    
    Button touchButton;
    Image touchImage;

    Tween touchTween;
    void Awake()
    {
        touchButton = transform.Find("InfoTouchButton").GetComponentInChildren<Button>();
        touchImage = transform.Find("InfoTouchImage").GetComponentInChildren<Image>();
        touchButton.onClick.AddListener(ClosePopup);
        versionText.text = "빌드버전 v" + Application.version;
    }

    public void ShowPopup()
    {
        gameObject.SetActive(true);
        touchButton.gameObject.SetActive(true);
        touchImage.gameObject.SetActive(true);
        StartTouchEffect();
    }
    public void ClosePopup()
    {
        gameObject.SetActive(false);
        touchButton.gameObject.SetActive(false);
        touchImage.gameObject.SetActive(false);
        AudioManager.Instance.SFX.PlayButtonClick();
        StopTouchEffect();
    }

    public void StartTouchEffect()
    {
        if (touchImage == null) return;

        touchTween?.Kill();

        touchTween = touchImage.transform.DOScale(1.7f, 0.8f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopTouchEffect()
    {
        if (touchTween != null && touchTween.IsActive())
        {
            touchTween.Kill();
            touchImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
}
