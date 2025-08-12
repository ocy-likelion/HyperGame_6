using Unity.VisualScripting;
using UnityEngine;

public class TitleState : IGameState
{
    public void OnEnter()
    {
        SceneController.TransitionToScene(SceneState.Title);
    }

    public void OnUpdate()
    {
        
    }
    
    public void OnExit()
    {
        
    }
}