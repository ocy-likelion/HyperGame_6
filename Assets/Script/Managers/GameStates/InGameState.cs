using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGameState : IGameState
{
    public void OnEnter()
    { 
       GameManager.Instance.StartCoroutine(StartGame());
       UIManager.Instance.inGameUIController.ShowInGameUI();
    }
    
    public void OnUpdate()
    {
        // TODO: 유저의 최고기록 불러오기 (임시: 'K'를 누르면 연출 재생)
        // New Record시, 점수판에 New Record Image 연출 재생
        if (Input.GetKeyDown(KeyCode.K))
        {
            InGameUIController.Instance.scoreUIController.ShowNewRecordImage();
        }
    }
    
    public void OnExit()
    {
       
    }

    IEnumerator StartGame()
    {
        yield return GameManager.Instance.inGameController.SetInitGame();
        yield return GameManager.Instance.inGameController.RunSequence();
    }
}