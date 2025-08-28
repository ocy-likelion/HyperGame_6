using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroBackGroundController : MonoBehaviour
{
    [SerializeField] private Image _backGroundImage;
    [SerializeField] private IntroUIController introUIController; // ¿¬°á

    private void OnEnable()
    {
        IntroUIController.OnIntroUIEnd += HandleIntroUIEnd;
    }

    private void OnDisable()
    {
        IntroUIController.OnIntroUIEnd -= HandleIntroUIEnd;
    }

    private void Awake()
    {
        _backGroundImage.color = Color.black;
        gameObject.SetActive(true);
    }

    private void HandleIntroUIEnd()
    {
        gameObject.SetActive(false);
    }
}
