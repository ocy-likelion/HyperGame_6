using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDaycycle : MonoBehaviour
{
    Quaternion initRotate;      // 초기 회전값 저장
    bool isPaused = false;      // 회전 일시 정지 여부

    void Start()
    {
        initRotate = transform.rotation;
    }

    void Update()
    {
        if (isPaused) return;

        // 하루 시간 기준 회전
        float rotateSpeed = 180f / TimeController.Instance._dayTime;
        transform.Rotate(0f, 0f, -rotateSpeed * Time.deltaTime);
    }

    public void PauseCycle() => isPaused = true;
    public void ResumeCycle() => isPaused = false;

    public void ResetCycle() => transform.rotation = initRotate;
}
