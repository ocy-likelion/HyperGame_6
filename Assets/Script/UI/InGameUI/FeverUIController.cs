using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverUIController : MonoBehaviour
{
    public Slider feverSlider;
    public float decreaseDuration = 5f; // 줄어드는 시간 (초)

    // 슬라이더 프레임 이미지
    [SerializeField] private Image sliderFrameImage; // SliderFrame의 Image
    [SerializeField] private Sprite normalFrame; // 일반 상태 프레임
    [SerializeField] private Sprite feverFrame; // 피버 상태 프레임

    // 피버 텍스트 이미지
    [SerializeField] private Image feverTextImage; // 피버 텍스트 이미지
    [SerializeField] private Sprite normalText; // 일반 상태 텍스트
    [SerializeField] private Sprite feverText; // 피버 상태 텍스트

    // 피버 이펙트 이미지
    [SerializeField] private Image feverEffectImage; // 피버 이펙트 이미지

    // DOTween 트윈 핸들
    private Tweener effectTweener;

    // 피버 슬라이더 감소 코루틴
    private Coroutine decreaseCoroutine;

    void Awake()
    {
        // 초기 설정
        feverSlider.value = 0f; // 슬라이더 초기값
        feverEffectImage.enabled = false; // 피버 이펙트 이미지 비활성화
        sliderFrameImage.sprite = normalFrame; // 일반 상태 프레임으로 설정
        feverTextImage.sprite = normalText; // 일반 상태 텍스트로 설정
        feverEffectImage.raycastTarget = false; // 피버 이펙트 이미지가 클릭 가능하도록 설정
    }
    void Update()
    {
        // 슬라이더 값이 1에 도달하면 감소 시작
        if (feverSlider.value >= 1f)
        {
            AudioManager.Instance.SFX.PlayFever(); // 피버 사운드 재생
            feverEffectImage.enabled = true; // 피버 이펙트 이미지 활성화
            sliderFrameImage.sprite = feverFrame; // 피버 상태 프레임으로 변경
            feverTextImage.sprite = feverText; // 피버 상태 텍스트로 변경
            // 이미 감소 중이면 중복 실행 방지
            if (decreaseCoroutine == null)
            {
                decreaseCoroutine = StartCoroutine(DecreaseSliderOverTime());
            }
            // 이펙트 깜빡이기 시작
            if (effectTweener == null || !effectTweener.IsActive())
            {
                StartBlinkEffect();
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

        feverTextImage.sprite = normalText; // 일반 상태 텍스트로 변경
        sliderFrameImage.sprite = normalFrame; // 일반 상태 프레임으로 변경
        feverSlider.value = 0f; // 완전히 0으로 맞추기
        feverEffectImage.enabled = false; // 피버 이펙트 이미지 비활성화

        // DOTween 효과 정리
        StopBlinkEffect();

        decreaseCoroutine = null; // 다음 실행 가능하도록 초기화
        GameManager.Instance.GetClassification().fever = false; // 피버 상태 해제
    }

    // 피버 이펙트 깜빡임 효과 시작 및 정지
    private void StartBlinkEffect()
    {
        feverEffectImage.color = new Color(1f, 1f, 1f, 1f); // 초기 알파값 설정

        effectTweener = feverEffectImage
            .DOFade(0f, 0.5f) // 0.5초 동안 알파 0으로
            .SetLoops(-1, LoopType.Yoyo) // 무한 반복 (깜빡임)
            .SetEase(Ease.InOutSine);
    }

    private void StopBlinkEffect()
    {
        if (effectTweener != null && effectTweener.IsActive())
        {
            effectTweener.Kill();
            effectTweener = null;
        }
        // 알파값 원래대로
        feverEffectImage.color = new Color(1f, 1f, 1f, 1f);
    }
}
