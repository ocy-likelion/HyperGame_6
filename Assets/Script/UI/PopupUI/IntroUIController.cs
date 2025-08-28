using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IntroUIController : PopupController
{
    [SerializeField] private Image IntroUI;
    [SerializeField] private GameObject backGroundObject;

    public void InitUI()
    {
        IntroUI.color = Color.black;
        gameObject.SetActive(true);
        if (backGroundObject != null)
            backGroundObject.SetActive(true);

    }

    protected override void OnEnable()
    {
            //
    }

    protected override void OnDisable()
    {
        //
    }
    
    public void ShowPopup()
    {
        base.ShowPopup(gameObject);
    }
    
    public void ClosePopup()
    {
        base.ClosePopup(gameObject);
    }

    public IEnumerator InitIntroUI()
    {
        gameObject.SetActive(true);
        var introEnd = false;
        IntroUI.DOColor(Color.white, 1f);
        yield return new WaitForSeconds(5f);
        IntroUI.DOColor(Color.black, 1f).OnComplete(()=>introEnd = true);
        yield return new WaitUntil(() => introEnd);
        gameObject.SetActive(false);
        if (backGroundObject != null)
            backGroundObject.SetActive(false);
    }
}
