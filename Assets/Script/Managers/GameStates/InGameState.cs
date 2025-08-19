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
       
       // BGM 재생
       AudioManager.Instance.BGM.PlayBGMByState(GameManager.Instance.GetGameState());
    }
    
    public void OnUpdate()
    {
        
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