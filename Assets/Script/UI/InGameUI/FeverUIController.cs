using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverUIController : MonoBehaviour
{
    public Slider feverSlider;
    public float decreaseDuration = 5f; // 줄어드는 시간 (초)

    private Coroutine decreaseCoroutine;

    void Update()
    {
        // 슬라이더 값이 1에 도달하면 감소 시작
        if (feverSlider.value >= 1f)
        {
            // 이미 감소 중이면 중복 실행 방지
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

        feverSlider.value = 0f; // 완전히 0으로 맞추기
        decreaseCoroutine = null; // 다음 실행 가능하도록 초기화
        GameManager.Instance.GetClassification().fever = false; // 피버 상태 해제
    }

}
