using System.Collections.Generic;
using UnityEngine;

public class InGameState : IGameState
{
    public void OnEnter()
    {
        SceneController.TransitionToScene(SceneState.InGame, 
            GameManager.Instance.inGameController.SetInitGame,
            GameManager.Instance.inGameController.RunSequence
            );
       UIManager.Instance.inGameUIController.ShowInGameUI();
    }
    
    public void OnUpdate()
    {
        
    }
    
    public void OnExit()
    {
       
    }
}