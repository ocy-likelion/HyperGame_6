using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitThreeSecondsUI : MonoBehaviour
{
    public TMP_Text SecondsText; // UI에 표시할 텍스트 컴포넌트

    public IEnumerator WaitThreeSeconds()
    {
        SecondsText.gameObject.SetActive(true); // 텍스트 활성화
        Time.timeScale = 0f; // 게임 시간 정지
        SecondsText.text = "3"; // 3초 표시
        yield return new WaitForSecondsRealtime(1f);
        SecondsText.text = "2"; // 2초 표시
        yield return new WaitForSecondsRealtime(1f);
        SecondsText.text = "1"; // 1초 표시
        yield return new WaitForSecondsRealtime(1f);
        SecondsText.text = "Start!"; // 시작 표시
        yield return new WaitForSecondsRealtime(1f);
        SecondsText.gameObject.SetActive(false); // 텍스트 숨김
        Time.timeScale = 1f; // 게임 시간 재개
    }
}
