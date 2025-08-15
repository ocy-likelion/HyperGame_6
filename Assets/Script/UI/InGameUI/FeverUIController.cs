using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverUIController : MonoBehaviour
{
    public Slider feverSlider;
    public float decreaseDuration = 5f; // �پ��� �ð� (��)

    private Coroutine decreaseCoroutine;

    void Update()
    {
        // �����̴� ���� 1�� �����ϸ� ���� ����
        if (feverSlider.value >= 1f)
        {
            // �̹� ���� ���̸� �ߺ� ���� ����
            if (decreaseCoroutine == null)
            {
                decreaseCoroutine = StartCoroutine(DecreaseSliderOverTime());
            }
        }
    }

    IEnumerator DecreaseSliderOverTime()
    {
        float startValue = feverSlider.value;
        float elapsed = 0f;

        while (elapsed < decreaseDuration)
        {
            elapsed += Time.deltaTime;
            feverSlider.value = Mathf.Lerp(startValue, 0f, elapsed / decreaseDuration);
            yield return null;
        }

        feverSlider.value = 0f; // ������ 0���� ���߱�
        decreaseCoroutine = null; // ���� ���� �����ϵ��� �ʱ�ȭ
        GameManager.Instance.GetClassification().fever = false; // �ǹ� ���� ����
    }

}
