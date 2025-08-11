using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classification : Singleton<Classification>
{
    bool obstacle; //��ֹ� ���� true: ��ֹ� ����, false: ��ֹ� ����
    bool clean; //�ݷ���� true: �ݷ���� ����, false: �ݷ���� ����
    public bool confirm; //���� ���� true: ���ι�ư Ŭ��, false: �ݷ���ư Ŭ��
    bool success; //�з� ���� ���� true: ����, false: ����
    int playTime = 60; //�ϰ��ð�
    int day = 1; //�ϰ� ��¥
    int combo = 0; //�޺� Ƚ��
    int maxCombo = 0; //�ִ� �޺� Ƚ��
    float feverValue = 0; //�ǹ� ������
    float scoreMag = 1.0f; //���� ����
    int score = 0; //����

    public void scoreMagnification()
    {
        switch(combo)
        {
            case int n when (n < 5):
                scoreMag = 1.0f; //�޺� ����
                break;
            case int n when (n >= 5 && n <=10):
                scoreMag = 1.1f; //�޺� 1.1��
                break;
            case int n when (n >= 11 && n <=20):
                scoreMag = 1.2f; //�޺� 1.2��
                break;
            case int n when (n >= 21 && n <= 30):
                scoreMag = 1.3f; //�޺� 1.3��
                break;
            case int n when (n >= 31 && n <= 40):
                scoreMag = 1.4f; //�޺� 1.4��
                break;
            case int n when (n >= 41 && n <= 50):
                scoreMag = 1.5f; //�޺� 1.5��
                break;
            case int n when (n >= 51 && n <= 60):
                scoreMag = 1.6f; //�޺� 1.6��
                break;
            case int n when (n >= 61 && n <= 70):
                scoreMag = 1.7f; //�޺� 1.7��
                break;
            case int n when (n >= 71 && n <= 80):
                scoreMag = 1.8f; //�޺� 1.8��
                break;
            case int n when (n >= 81 && n <= 90):
                scoreMag = 1.9f; //�޺� 1.9��
                break;
            case int n when (n >= 91 && n <= 100):
                scoreMag = 2.0f; //�޺� 2��
                break;
            case int n when (n >= 101):
                scoreMag = 2.5f; //�޺� 2.5�� 
                break;
        }
    } //���� ���� ����
    public void DocumentClassification() //���� �з� �޼ҵ�
    {
        if(obstacle) // ��ֹ��� ���� �� 
        {
            success = false;
            playTime -= 5 * day; //�ϰ��ð� ����
            combo = 0; //�޺� �ʱ�ȭ
            Debug.Log("�з� ����! ��ֹ� ����. �ϰ��ð� ����: " + playTime + ", ���� �޺�: " + combo + ", �ִ� �޺�: " + maxCombo + "���� ����: " + scoreMag);
        }
        else // ��ֹ��� ���� ��
        {
            if(clean) // �ݷ���Ұ� ���� ��
            {
                if(confirm) // ���� ��ư Ŭ�� ��
                {
                    success = true;
                    playTime += 1 * day; //�ϰ��ð� ����
                    combo += 1; //�޺� ����
                    feverValue += 3 * scoreMag; //�ǹ� ������ ����
                    score += (int)((1 * day) * scoreMag); //���� ����
                    scoreMagnification(); //���� ���� ����
                    if (combo > maxCombo)
                    {
                        maxCombo = combo; //�ִ� �޺� ����
                    }
                    
                    Debug.Log("�з� ����! �ϰ��ð� ����: " + playTime + ", ���� �޺�: " + combo + ", �ִ� �޺�: " + maxCombo + "���� ����: " + scoreMag);
                }
                else // �ݷ� ��ư Ŭ�� ��
                {
                    success = false;
                    playTime -= 5 * day; //�ϰ��ð� ����
                    combo = 0; //�޺� �ʱ�ȭ
                    scoreMagnification(); //���� ���� ����
                    feverValue -= (float)(feverValue * 0.1); //�ǹ� ������ ����
                    Debug.Log("�з� ����! �ϰ��ð� ����: " + playTime + ", ���� �޺�: " + combo + ", �ִ� �޺�: " + maxCombo + "���� ����: " + scoreMag);
                }
            }
            else // �ݷ���Ұ� ���� ��
            {
                if(confirm) // ���� ��ư Ŭ�� ��
                {
                    success = false;
                    playTime -= 5 * day; //�ϰ��ð� ����
                    combo = 0; //�޺� �ʱ�ȭ
                    scoreMagnification(); //���� ���� ����
                    feverValue -= (float)(feverValue * 0.1); //�ǹ� ������ ����
                    Debug.Log("�з� ����! �ݷ���� ����. �ϰ��ð� ����: " + playTime + ", ���� �޺�: " + combo + ", �ִ� �޺�: " + maxCombo + "���� ����: " + scoreMag);
                }
                else // �ݷ� ��ư Ŭ�� ��
                {
                    success = true;
                    playTime += 1 * day; //�ϰ��ð� ����
                    combo += 1; //�޺� ����
                    feverValue += 3 * scoreMag; //�ǹ� ������ ����
                    score += (int)((1 * day) * scoreMag); //���� ����
                    scoreMagnification(); //���� ���� ����
                    if (combo > maxCombo)
                    {
                        maxCombo = combo; //�ִ� �޺� ����
                    }
                    
                    Debug.Log("�з� ����! �ݷ���� ����. �ϰ��ð� ����: " + playTime + ", ���� �޺�: " + combo + ", �ִ� �޺�: " + maxCombo + "���� ����: " + scoreMag);
                }
            }
        }
    }
}
