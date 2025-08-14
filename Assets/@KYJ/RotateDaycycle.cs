using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDaycycle : MonoBehaviour
{
    Quaternion initRotate;      // �ʱ� ȸ���� ����
    bool isPaused = false;      // ȸ�� �Ͻ� ���� ����

    void Start()
    {
        initRotate = transform.rotation;
    }

    void Update()
    {
        if (isPaused) return;

        // �Ϸ� �ð� ���� ȸ��
        float rotateSpeed = 180f / TimeController.Instance._dayTime;
        transform.Rotate(0f, 0f, -rotateSpeed * Time.deltaTime);
    }

    public void PauseCycle() => isPaused = true;
    public void ResumeCycle() => isPaused = false;

    public void ResetCycle() => transform.rotation = initRotate;
}
