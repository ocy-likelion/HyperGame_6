using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public float maxTime; // �ִ� �ð�
    float currentTime; // ���� �ð�

    public Text timeText; // Timer UI �ؽ�Ʈ
    public Text dayText; // Day UI �ؽ�Ʈ
    
    bool isTimeRunning = false; // Ÿ�̸� �۵� ����
    public bool timeOut = false; // Ÿ�Ӿƿ� ����

    public int day = 1; // ���� Day

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

        // ���� �ð� �ؽ�Ʈ�� ǥ�� (�Ҽ��� 1�ڸ�)
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
            dayText.text = $"{day}��";
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
        // Ÿ�Ӿƿ� �� ����
        timeOut = true;
        day++; // Day ����

        // Day UI ����
        if (dayText != null)
            dayText.text = $"{day}��";

        // Ÿ�̸� �����
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

        /* ���Ѽ� �����Ϸ��� �ּ� ����
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
