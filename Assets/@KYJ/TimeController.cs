using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : Singleton<TimeController>
{
    [Header("타이머 설정")]
    [SerializeField] float timer = 60f;       // 타이머 시간
    bool isTimeRunning = false;                 // 타이머 작동 여부
    float remainedTimerTime;                     // 타이머 남은 시간

    [Header("하루 길이 설정")]
    [SerializeField] float dayTime = 120f;    // 하루 길이
    int day = 1;                                // 현재 일수
    float elapsedDayTime = 0f;                  // 하루 경과 시간

    public float _remainedTimerTime => remainedTimerTime;
    public float _remainedDayTime => dayTime - elapsedDayTime; // 하루 남은 시간
    public float _dayTime => dayTime;
    public int _day => day;

    public void SetRemainedTimer(float value) => remainedTimerTime = Mathf.Max(0f, value);
    public void SetDay(int value) => day = Mathf.Max(1, value);

    public void InitTimeController()
    {
        remainedTimerTime = timer;
        elapsedDayTime = 0f;
        UpdateTimeUI();
        UpdateDayUI();
    }

    public void StartRunningTimer()
    {
        if (isTimeRunning) return;
        isTimeRunning = true;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (isTimeRunning)
        {
            // 타이머 감소
            remainedTimerTime -= Time.deltaTime;
            if (remainedTimerTime < 0f) remainedTimerTime = 0f;

            // 하루 시간 증가
            elapsedDayTime += Time.deltaTime;
            if (elapsedDayTime > dayTime) elapsedDayTime = dayTime;

            UpdateTimeUI();
            UpdateDayUI();

            // 하루가 지났는지 체크
            if (elapsedDayTime >= dayTime)
            {
                HandleDayEnd();
            }

            yield return null;
        }
    }

    void HandleDayEnd()
    {
        day++;                  // 하루 일수 증가
        elapsedDayTime = 0f;    // 하루 남은 시간 초기화
        remainedTimerTime = timer; // 타이머 초기화

        Debug.Log($"하루 지남. {day}일차");
    }

    public void ResetTimer()
    {
        remainedTimerTime = timer;
        elapsedDayTime = 0f;
        isTimeRunning = false;
        UpdateTimeUI();
        UpdateDayUI();
    }

    void UpdateTimeUI()
    {
        if (UIManager.Instance.inGameUIController.timeUIController.timerText is var timeText && timeText != null)
            timeText.text = remainedTimerTime.ToString("F1");
    }

    void UpdateDayUI()
    {
        if (UIManager.Instance.inGameUIController.timeUIController.dayText is var dayText && dayText != null)
            dayText.text = $"{day} Day";
    }
}
