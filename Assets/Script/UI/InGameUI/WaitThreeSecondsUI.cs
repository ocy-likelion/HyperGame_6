using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaitThreeSecondsUI : MonoBehaviour
{
    public Image CountdownImage; // 카운트다운 이미지 UI
    public Sprite[] CountdownSprites; // [0] = 3, [1] = 2, [2] = 1, [3] = Start
    public Image TouchBlockPanel; // 터치 막는 패널

    public IEnumerator WaitThreeSeconds()
    {
        CountdownImage.gameObject.SetActive(true);
        TouchBlockPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;

        if (CountdownSprites.Length < 4)
        {
            Debug.LogError("CountdownSprites 배열에 4개의 스프라이트(3,2,1,Start)가 필요합니다.");
            yield break;
        }

        CountdownImage.sprite = CountdownSprites[0]; // 3
        AudioManager.Instance.SFX.PlayGameStart();
        yield return new WaitForSecondsRealtime(1f);

        CountdownImage.sprite = CountdownSprites[1]; // 2
        yield return new WaitForSecondsRealtime(1f);

        CountdownImage.sprite = CountdownSprites[2]; // 1
        yield return new WaitForSecondsRealtime(1f);

        CountdownImage.sprite = CountdownSprites[3]; // Start!
        yield return new WaitForSecondsRealtime(1f);

        CountdownImage.gameObject.SetActive(false);
        TouchBlockPanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}