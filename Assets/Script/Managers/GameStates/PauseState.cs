using UnityEngine;

public class PauseState : IGameState
{
    public void OnEnter()
    {
        Time.timeScale = 0f;
        //TODO: 일시정지 UI Show
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        Time.timeScale = 1f;
        //TODO: 일시정지 UI Hide
    }
}