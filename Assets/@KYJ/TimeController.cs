using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    // [Header("UI")]
    // public TMP_Text timeText; // 일과 시간 텍스트
    // public TMP_Text dayText;  // 진행 일수 텍스트

    [Header("초기 일과 시간 설정")]
    public float playTime = 60.0f; // 하루 일과 시간
    public float remainedTime; // 현재 남은 시간
    
    public float _remainedTime
    {
        get => remainedTime;
        set => remainedTime = value;
    }

    [Header("진행 일수")]
    public int day = 1; // 현재 진행 일수

    public int _day
    {
        get => day;
        set => day = value;
    }

    [Header("타임 아웃 여부")]
    public bool timeOut = false; // 타임아웃 (시간 종료) 여부
    bool isTimeRunning = false; // 타이머 동작 여부

    public void InitTimeController()
    {
        remainedTime = playTime; // 남은 시간 초기화
        UpdateTimeUI();          // 시간 UI 갱신
        UpdateDayUI();           // 일수 UI 갱신
    }

    public void StartRunningTimer()
    {
        if (isTimeRunning) return; //타이머가 이미 실행중이면 중단
        Debug.Log("StartRunningTimer");
        StartTime();             // 타이머 시작
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (isTimeRunning)// 타이머가 동작 중이 아닐 경우 업데이트 중지
        {
            remainedTime -= Time.deltaTime; // 시간 감소
            CheckTimeEnd();                 // 타이머 종료 체크
            UpdateTimeUI();                 // 남은 시간 UI 갱신
            UpdateDayUI();                  // 진행 일수 UI 갱신
            
            yield return null;
        }
    }

    public void StartTime() // 타이머 시작
    {
        isTimeRunning = true;
    }

    public void StopTime() // 타이머 정지
    {
        isTimeRunning = false;
    }
    
    public void ResetTime() // 타이머 및 상태 초기화
    {
        remainedTime = playTime;
        isTimeRunning = false;
        timeOut = false;
        UpdateTimeUI();
        UpdateDayUI();
    }

    void CheckTimeEnd() // 타이머가 0초가 되었는지 확인
    {
        if (remainedTime > 0f) return;

        remainedTime = 0f;
        StopTime();
        HandleTimeOut();
        GameManager.Instance.inGameController.Dispose();//게임 오버
    }

    void HandleTimeOut() // 타임아웃 처리: 일수 증가 및 상태 갱신
    {
        timeOut = true;
        day++;
    }

    void UpdateTimeUI() // 남은 시간 UI 갱신
    {
        if (UIManager.Instance.inGameUIController.timeUIController.workTimeText is var timeText && timeText != null)
            timeText.text = remainedTime.ToString("F1");
    }

    void UpdateDayUI() // 진행 일수 UI 갱신("1일" 형식)
    {
        if (UIManager.Instance.inGameUIController.timeUIController.dayText is var dayText && dayText != null)
            dayText.text = $"{day} Day";
    }
}