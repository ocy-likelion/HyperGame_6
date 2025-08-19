using Unity.VisualScripting;
using UnityEngine;

public class TitleState : IGameState
{
    public void OnEnter()
    {
        //SceneController.TransitionToScene(SceneState.Title);
        UIManager.Instance.titleUIController.ShowTitleUI();
        UIManager.Instance.titleUIController.ShowMainMenuUI();
        UIManager.Instance.titleUIController.ShowSubMenuUI();
        
        // BGM 재생
        AudioManager.Instance.BGM.PlayBGMByState(GameManager.Instance.GetGameState());
    }

    public void OnUpdate()
    {
        
    }
    
    public void OnExit()
    {
        UIManager.Instance.titleUIController.HideTitleUI();
        UIManager.Instance.titleUIController.HideMainMenuUI();
        UIManager.Instance.titleUIController.HideSubMenuUI();
    }
}