using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUIController : MonoBehaviour
{
    [SerializeField] GameObject clockHandle;
    [SerializeField] Image clockFrame;

    [Header("각도 변화량")]
    public float stepAngle = 30f;

    [Header("틱 간격")]
    public float tickInterval = 1.0f;

    [Header("경보 시간 설정")]
    public int setTime = 30;

    float targetZ;
    float timer;

    void Update()
    {
        if (!TimeController.Instance._isTimeRunning) return;

        ClockHandAnimation();
        ClockFrameColor();
    }

    public void ClockHandAnimation() // 시계 바늘 움직이는 기능
    {
        timer += Time.deltaTime;

        if (timer >= tickInterval) // 틱 간격이 지났을 때
        {
            // 목표 각도를 현재 각도에서 stepAngle 만큼 증가
            targetZ += -stepAngle; // 시계방향으로 회전하기위해 -를 붙임
            timer = 0f;
        }

        // 목표 각도로 보간
        clockHandle.transform.rotation = Quaternion.Lerp(clockHandle.transform.rotation, Quaternion.Euler(0, 0, targetZ), Time.deltaTime * 10f);
    }

    public void ClockFrameColor() // 시계 프레임 색상 변경 기능
    {
        float remainedTime = TimeController.Instance._remainedTimerTime;

        Color customGreen = new Color(0.525f, 0.89f, 0.208f); // #86E335
        
        if (remainedTime <= setTime) // 일과 시간이 30초 이하일 때
        {
            float t = Mathf.PingPong(Time.time * 2f, 1f); // 색상 변경 딜레이
            clockFrame.color = Color.Lerp(customGreen, Color.red, t); // 흰색에서 빨간색으로 보간

            /* SFX, VFX 등 추가할거 있으면 여기에 추가 및 수정하시면 됩니다 */
        }
        else
        {
            InitClockFrameColor(); // 기본 색상으로 설정
        }
    }
    
    public void InitClockFrameColor() // 시계 프레임 색상 초기화
    {
        clockFrame.color = new Color(0.525f, 0.89f, 0.208f);
    }
}