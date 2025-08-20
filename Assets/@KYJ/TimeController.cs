using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : Singleton<TimeController>
{
    [Header("타이머 길이")]
    [SerializeField] float constTimerValue = 60.0f;
    bool isTimeRunning = false; // 타이머 실행 중인지
    float remainedTimerTime; // 남은 일과 시간

    [Header("하루 길이")]
    [SerializeField] float dayTime = 120.0f;
    float elapsedDayTime = 0f; // 하루 중 몇시간 얼마나 지났는지

    [Header("현재 날짜")]
    [SerializeField] int day = 1;

    public float _remainedTimerTime => remainedTimerTime;
    public float _remainedDayTime => dayTime - elapsedDayTime; // 하루 남은 시간
    public float _dayTime => dayTime;
    public int _day => day;
    public bool _isTimeRunning => isTimeRunning;

    public void SetRemainedTimer(float value) => remainedTimerTime = Mathf.Max(0f, value);
    public void SetDay(int value) => day = Mathf.Max(1, value);

    public void InitTimeController()
    {
        ResetTimer();
    }

    public void StartRunningTimer()
    {
        if (isTimeRunning) return;
        isTimeRunning = true;
        StartCoroutine(Timer());
    }

    public void StopTime()
    {
        isTimeRunning = false;
        UIManager.Instance.inGameUIController.backGroundUIController.rotateDaycycle.PauseCycle();
    }

    IEnumerator Timer()
    {
        UIManager.Instance.inGameUIController.backGroundUIController.rotateDaycycle.ResumeCycle();
        
        while (isTimeRunning)
        {
            remainedTimerTime -= Time.deltaTime; // 일과 시간 감소

            if (remainedTimerTime <= 0f)
            {
                remainedTimerTime = 0f; // 일과 시간이 0초 이하로 내려가면 0으로 설정
                UpdateTimeUI();
                GameManager.Instance.inGameController.Dispose(); // 일과 시간이 0초 이하면 게임 종료 처리
                StopTime();
                yield break;
            }

            elapsedDayTime += Time.deltaTime; // 남은 하루 길이 계산
            if (elapsedDayTime >= dayTime) HandleDayEnd();

            UpdateTimeUI();
            UpdateDayUI();

            yield return null;
        }
    }

    void HandleDayEnd() // 하루가 끝나면 일과 시간 및 남은 시간 초기화
    {
        day++;                  // 하루 일수 증가
        elapsedDayTime = 0f;    // 하루 남은 시간 초기화
        remainedTimerTime = constTimerValue; // 타이머 초기화
        // Debug.Log($"하루가 지났습니다. 현재 {day}일차");
    }

    public void ResetTimer()
    {
        remainedTimerTime = constTimerValue;
        elapsedDayTime = 0f;
        day = 1;
        isTimeRunning = false;
        UIManager.Instance.inGameUIController.backGroundUIController.rotateDaycycle.ResetCycle();
        UpdateTimeUI();
        UpdateDayUI();
    }

    void UpdateTimeUI()
    {
        if (UIManager.Instance.inGameUIController.timeUIController.timerText is var timeText && timeText != null)
            timeText.text = $"{remainedTimerTime:F0}";
    }

    void UpdateDayUI()
    {
        if (UIManager.Instance.inGameUIController.timeUIController.dayText is var dayText && dayText != null)
            dayText.text = $"{day}";
    }
}
