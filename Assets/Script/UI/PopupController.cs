using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [Header("팝업 관리")]
    [SerializeField] private List<GameObject> popups;

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