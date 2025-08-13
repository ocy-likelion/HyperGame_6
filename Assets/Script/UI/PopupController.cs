using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    // [Header("팝업 관리")]
    // [SerializeField] private List<GameObject> popups;

    //활성화 시 뒷배경 켜기
    void OnEnable()
    {
        UIManager.Instance.ShowBackgroundImage(true);
    }

    void OnDisable()
    {
        UIManager.Instance.ShowBackgroundImage(false);
    }
    
    // 팝업 열기
    public void ShowPopup(GameObject popup)
    {
        if (popup != null)
            popup.SetActive(true);
    }

    // 팝업 닫기
    public void ClosePopup(GameObject popup)
    {
        if (popup != null)
            popup.SetActive(false);
    }
}