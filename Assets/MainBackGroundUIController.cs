using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainBackGroundUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void Awake()
    {
        scoreText.text = "_________";
    }

    //시작 시 초기화
    public void InitUI()
    {
        var bestScore = (int)PlayerPrefs.GetFloat("BestScore", 0f);
        scoreText.text = bestScore == 0 ? "_________" : bestScore.ToString();
    }
    
    //게임 중 신기록 반영
    public void UpdateBestScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
