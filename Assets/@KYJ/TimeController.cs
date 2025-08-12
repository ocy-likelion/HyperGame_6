using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public float maxTime; // 최대 시간
    float currentTime; // 현재 시간

    public Text timeText; // Timer UI 텍스트
    public Text dayText; // Day UI 텍스트
    
    bool isTimeRunning = false; // 타이머 작동 여부
    public bool timeOut = false; // 타임아웃 여부

    public int day = 1; // 현재 Day

    void Update()
    {
        if (isTimeRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isTimeRunning = false;
                TimeOut();
            }
        }

        // 현재 시간 텍스트로 표시 (소수점 1자리)
        if (timeText != null)
        {
            timeText.text = currentTime.ToString("F1");
        }
    }

    void Start()
    {
        currentTime = maxTime;
        if (timeText != null)
            timeText.text = currentTime.ToString("F1");
        if (dayText != null)
            dayText.text = $"{day}일";
    }

    public void StartTimer()
    {
        if (timeOut) 
            timeOut = !timeOut;

        if (currentTime <= 0f) 
            currentTime = maxTime;

        isTimeRunning = true;
    }

    public void StopTimer()
    {
        isTimeRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = maxTime;
        isTimeRunning = false;
        timeOut = false;
    }

    void TimeOut()
    {
        // 타임아웃 시 로직
        timeOut = true;
        day++; // Day 증가

        // Day UI 갱신
        if (dayText != null)
            dayText.text = $"{day}일";

        // 타이머 재시작
        currentTime = maxTime;
        isTimeRunning = true;
    }

    public float CurrentTime
    {
        get { return currentTime; }
    }

    public void AddTime(float addValue)
    {
        currentTime += addValue;

        /* 상한선 설정하려면 주석 해제
        if (currentTime > maxTime) 
            currentTime = maxTime;
        */
    }

    public void MinusTime(float minusValue)
    {
        currentTime -= minusValue;
        if (currentTime < 0f)
        {
            currentTime = 0f;
            isTimeRunning = false;
            TimeOut();
        }
    }
}
