using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [Header("패널 관리")]
    [SerializeField] private List<GameObject> panels;

    // 지정한 패널만 열고 나머지는 닫음
    public void OpenPanel(GameObject panel)
    {
        foreach (var p in panels)
            p.SetActive(p == panel);
    }
}
