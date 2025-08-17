using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void OnClickConfirmButton() //승인버튼클릭
    {
        GameManager.Instance.GetClassification().confirm = true; //승인버튼 클릭시 서류 승인
        GameManager.Instance.GetClassification().DocumentClassification(); // 서류 분류 메소드 호출
    }

    public void OnClickRejectButton() //반려버튼클릭
    {
        GameManager.Instance.GetClassification().confirm = false; //반려버튼 클릭시 서류 반려
        GameManager.Instance.GetClassification().DocumentClassification(); // 서류 분류 메소드 호출
    }
}
