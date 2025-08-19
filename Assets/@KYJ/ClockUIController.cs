using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class ClockUIController : MonoBehaviour
{
    public GameObject clockHandle; // 시계 바늘
    public Image clockFrame; // 시계 프레임

    public float stepAngle = 30f; // 각도 변화량 (단위 = n도)
    public float tickInterval = 0.5f; // 틱 간격 (단위 = n초)
    float targetZ; // 목표 Z축 회전값
    float timer; // 시계 바늘 조정용 타이머

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
        //Debug.Log(remainedTime);

        if (remainedTime <= 30) // 일과 시간이 30초 이하일 때
        {
            // SFX, VFX 등 추가할거 있으면 여기를 수정하시면 됩니다

            float t = Mathf.PingPong(Time.time * 2f, 1f); // 색상 변경 딜레이
            clockFrame.color = Color.Lerp(Color.white, Color.red, t); // 흰색에서 빨간색으로 보간
        }
        else
        {
            clockFrame.color = Color.white; // 기본 색상으로 설정
        }
    }
}