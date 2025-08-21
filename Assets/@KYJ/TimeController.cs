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
    private float currentDecreaseInterval; // 현재 감소 간격

    //[Header("하루 길이")]
    private float dayTime;
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
        
        float timerAccumulator = 0f; // 누적 시간

        while (isTimeRunning)
        {
            timerAccumulator += Time.deltaTime;

            // 감소주기 변수 사용
            while (timerAccumulator >= currentDecreaseInterval)
            {
                remainedTimerTime -= 1f;
                timerAccumulator -= currentDecreaseInterval;

                if (remainedTimerTime <= 0f)
                {
                    remainedTimerTime = 0f;
                    UpdateTimeUI();
                    GameManager.Instance.inGameController.Dispose();
                    StopTime();
                    yield break;
                }

                UpdateTimeUI();
            }

            elapsedDayTime += Time.deltaTime;
            if (elapsedDayTime >= dayTime) HandleDayEnd();
            UpdateDayUI();

            yield return null;
        }
    }

    void HandleDayEnd() // 하루가 끝나면 일과 시간 및 남은 시간 초기화
    {
        day++;                  // 하루 일수 증가
        elapsedDayTime = 0f;    // 하루 남은 시간 초기화
        
        // day가 바뀌었으니 감소 간격 갱신
        currentDecreaseInterval = DifficultyManager.Instance.GetTimeDecreaseRate(day);
    }

    public void ResetTimer()
    {
        remainedTimerTime = constTimerValue;
        elapsedDayTime = 0f;
        day = 1;
        
        // 하루당 시간 할당
        dayTime = DifficultyManager.Instance.DayTime;
        // 초기 감소 간격 할당
        currentDecreaseInterval = DifficultyManager.Instance.GetTimeDecreaseRate(day);
        
        isTimeRunning = false;
        UIManager.Instance.inGameUIController.backGroundUIController.rotateDaycycle.ResetCycle();
        UpdateTimeUI();
        UpdateDayUI();
    }

    public void UpdateTimeUI()
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
