using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIController : PopupController
{
    private float delayTime = 3f;
    
    public void ShowPopup()
    {
        base.ShowPopup(gameObject);
    }
    
    public void ClosePopup()
    {
        base.ClosePopup(gameObject);
    }

    public IEnumerator ShowSequence()
    {
        ShowPopup();
        
        float elapsedTime = 0f;
        
        while (elapsedTime <= delayTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        ClosePopup();
    }
}
