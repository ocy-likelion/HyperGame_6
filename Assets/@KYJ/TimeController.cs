using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [Header("타이머 설정")]
    public float maxTime = 60f; // 타이머의 최대 시간(초)
    private float currentTime; // 남은 시간
    private bool isTimeRunning = false; // 타이머가 동작 중인지 여부
    public bool timeOut = false; // 타임아웃(시간 종료) 여부

    [Header("UI")]
    public Text timeText; // 남은 시간을 표시할 UI 텍스트
    public Text dayText;  // Day를 표시할 UI 텍스트

    [Header("게임 진행")]
    public int day = 1; // 현재 Day 값
    
    void Start()
    {
        currentTime = maxTime; // 타이머를 최대 시간으로 초기화
        UpdateTimeUI();        // 시간 UI 갱신
        UpdateDayUI();         // Day UI 갱신
    }

    void Update()
    {
        if (isTimeRunning)
        {
            currentTime -= Time.deltaTime; // 시간 감소
            CheckTimerEnd();               // 타이머 종료 체크
            UpdateTimeUI();                // 시간 UI 갱신
        }
    }

    // 타이머 시작
    public void StartTimer()
    {
        if (timeOut)
            timeOut = false; // 타임아웃 상태 초기화

        if (currentTime <= 0f)
            currentTime = maxTime; // 시간이 0이면 최대 시간으로 초기화

        isTimeRunning = true; // 타이머 동작 시작
    }

    // 타이머 정지
    public void StopTimer()
    {
        isTimeRunning = false;
    }

    // 타이머 및 상태 초기화
    public void ResetTimer()
    {
        currentTime = maxTime;
        isTimeRunning = false;
        timeOut = false;
        UpdateTimeUI();
    }

    // 타이머 시간 증가 (외부에서 호출)
    public void AddTime(float value)
    {
        currentTime += value;
        if (currentTime > maxTime)
            currentTime = maxTime; // 최대 시간 초과 방지
        CheckTimerEnd();
        UpdateTimeUI();
    }

    // 타이머 시간 감소 (외부에서 호출)
    public void MinusTime(float value)
    {
        currentTime -= value;
        CheckTimerEnd();
        UpdateTimeUI();
    }

    // 타이머가 종료(0 이하)되었는지 체크
    private void CheckTimerEnd()
    {
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isTimeRunning = false;
            HandleTimeOut(); // 타임아웃 처리
        }
    }

    // 타임아웃 처리: Day 증가, UI 갱신, 타이머 자동 재시작
    private void HandleTimeOut()
    {
        timeOut = true;
        day++;             // Day 증가
        UpdateDayUI();     // Day UI 갱신
        currentTime = maxTime; // 타이머 재시작
        isTimeRunning = true;
        UpdateTimeUI();
    }

    // 남은 시간 UI 갱신
    private void UpdateTimeUI()
    {
        if (timeText != null)
            timeText.text = currentTime.ToString("F1");
    }

    // Day UI 갱신
    private void UpdateDayUI()
    {
        if (dayText != null)
            dayText.text = $"{day}일";
    }

    // 현재 남은 시간 반환 (읽기 전용 프로퍼티)
    public float CurrentTime => currentTime;
}
