using Unity.VisualScripting;
using UnityEngine;

public class TitleState : IGameState
{
    public void OnEnter()
    {
        SceneController.TransitionToScene(SceneState.Title);
        UIManager.Instance.titleUIController.ShowTitleUI();
        UIManager.Instance.titleUIController.ShowMainMenuUI();
        UIManager.Instance.titleUIController.ShowSubMenuUI();
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