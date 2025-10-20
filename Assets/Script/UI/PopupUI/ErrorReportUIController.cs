using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorReportUIController : PopupController
{
    public TMP_Text errorText;
    public Button retryProcessButton;
    public event Action OnRetryProcess; //

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        //셋업 초기화
        errorText.text = "";
        retryProcessButton.onClick.AddListener(RetryProcess);
    }
    
    public void ShowPopup()
    {
        base.ShowPopup(gameObject);
    }

    /// <summary>
    /// 팝업을 닫습니다.
    /// </summary>
    /// <param name="autoClose"> true로 설정하면 2초 후 자동으로 팝업이 닫힙니다.</param>
    public void ClosePopup(bool autoClose = false)
    {
        if (autoClose) StartCoroutine(DelayAction(2f, () => base.ClosePopup(gameObject)));
        else base.ClosePopup(gameObject);
    }

    IEnumerator DelayAction(float delay, Action action)
    {
        var elapsedTime = 0f;
        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        action?.Invoke();
    }

    public void SetErrorText(string msg)
    {
        errorText.text = msg;
    }

    public string GetErrorText()
    {
        return errorText.text;
    }
    
    //터치하여 동작 재시작, 플레이어에게 네트워크를 확인 후 동작해달라는 메시지 함께 첨부
    private void RetryProcess()
    {
        if (OnRetryProcess == null) return;
        
        //재호출 후 초기화
        OnRetryProcess?.Invoke();
        OnRetryProcess = null;
        
        //팝업 닫기
        ClosePopup();
    }
}
