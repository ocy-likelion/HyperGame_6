using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _gameTutorialButton;
    [SerializeField] private GameObject _tutorialPanel;

    private void Awake()
    {
        //버튼 클릭이벤트 등록
        _gameStartButton.onClick.AddListener(OnClickGameStartButton);
        _gameTutorialButton.onClick.AddListener(OnClickGameTutotialButton);
    }
    
    public void OnClickGameStartButton()
    {
        GameManager.Instance.GoToInGame();
    }

    public void OnClickGameTutotialButton()
    {
        _tutorialPanel.SetActive(true);
    }
}
