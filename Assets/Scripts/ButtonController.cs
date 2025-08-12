using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void OnClickConfirmButton() //승인버튼클릭
    {
        Classification.Instance.confirm = true; //승인버튼 클릭시 서류 승인
        Classification.Instance.DocumentClassification(); // 서류 분류 메소드 호출
    }

    public void OnClickRejectButton() //반려버튼클릭
    {
        Classification.Instance.confirm = false; //반려버튼 클릭시 서류 반려
        Classification.Instance.DocumentClassification(); // 서류 분류 메소드 호출
    }
}
