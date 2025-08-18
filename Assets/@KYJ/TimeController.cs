using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : Singleton<TimeController>
{
    [Header("타이머 설정")]
    [SerializeField] float timer = 60f;       // 일과 시간 타이머
    bool isTimeRunning = false;                 // 일과 시간 타이머 작동 여부
    float remainedTimerTime;                     // 남은 일과 시간

    [Header("하루 길이 설정")]
    [SerializeField] float dayTime = 120f;    // 하루 길이
    int day = 1;                                // 현재 일수
    float elapsedDayTime = 0f;                  // 하루 경과 시간

    public float _remainedTimerTime => remainedTimerTime;
    public float _remainedDayTime => dayTime - elapsedDayTime; // 하루 남은 시간
    public float _dayTime => dayTime;
    public int _day => day;
    public bool _isTimeRunning => isTimeRunning;

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

    public void StopTime()
    {
        isTimeRunning = false;
        RotateDaycycle.Instance.PauseCycle();
    }

    IEnumerator Timer()
    {
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
        remainedTimerTime = timer; // 타이머 초기화
        Debug.Log($"하루가 지났습니다. 현재 {day}일차");
    }

    public void ResetTimer()
    {
        remainedTimerTime = timer;
        elapsedDayTime = 0f;
        isTimeRunning = false;
        RotateDaycycle.Instance.ResetCycle();
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
