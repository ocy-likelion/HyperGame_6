using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void OnClickConfirmButton() //���ι�ưŬ��
    {
        Classification.Instance.confirm = true; //���ι�ư Ŭ���� ���� ����
        Classification.Instance.DocumentClassification(); // ���� �з� �޼ҵ� ȣ��
    }

    public void OnClickRejectButton() //�ݷ���ưŬ��
    {
        Classification.Instance.confirm = false; //�ݷ���ư Ŭ���� ���� �ݷ�
        Classification.Instance.DocumentClassification(); // ���� �з� �޼ҵ� ȣ��
    }
}
