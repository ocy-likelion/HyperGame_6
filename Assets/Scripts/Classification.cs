using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classification : Singleton<Classification>
{
    bool obstacle; //장애물 유무 true: 장애물 있음, false: 장애물 없음
    bool clean; //반려요소 true: 반려요소 없음, false: 반려요소 있음
    public bool confirm; //승인 여부 true: 승인버튼 클릭, false: 반려버튼 클릭
    bool success; //분류 성공 여부 true: 성공, false: 실패
    int playTime = 60; //일과시간
    int day = 1; //일과 날짜
    int combo = 0; //콤보 횟수
    int maxCombo = 0; //최대 콤보 횟수
    float feverValue = 0; //피버 게이지
    float scoreMag = 1.0f; //점수 배율
    int score = 0; //점수

    public void scoreMagnification()
    {
        switch(combo)
        {
            case int n when (n < 5):
                scoreMag = 1.0f; //콤보 없음
                break;
            case int n when (n >= 5 && n <=10):
                scoreMag = 1.1f; //콤보 1.1배
                break;
            case int n when (n >= 11 && n <=20):
                scoreMag = 1.2f; //콤보 1.2배
                break;
            case int n when (n >= 21 && n <= 30):
                scoreMag = 1.3f; //콤보 1.3배
                break;
            case int n when (n >= 31 && n <= 40):
                scoreMag = 1.4f; //콤보 1.4배
                break;
            case int n when (n >= 41 && n <= 50):
                scoreMag = 1.5f; //콤보 1.5배
                break;
            case int n when (n >= 51 && n <= 60):
                scoreMag = 1.6f; //콤보 1.6배
                break;
            case int n when (n >= 61 && n <= 70):
                scoreMag = 1.7f; //콤보 1.7배
                break;
            case int n when (n >= 71 && n <= 80):
                scoreMag = 1.8f; //콤보 1.8배
                break;
            case int n when (n >= 81 && n <= 90):
                scoreMag = 1.9f; //콤보 1.9배
                break;
            case int n when (n >= 91 && n <= 100):
                scoreMag = 2.0f; //콤보 2배
                break;
            case int n when (n >= 101):
                scoreMag = 2.5f; //콤보 2.5배 
                break;
        }
    } //점수 배율 조정
    public void DocumentClassification() //서류 분류 메소드
    {
        if(obstacle) // 장애물이 있을 때 
        {
            success = false;
            playTime -= 5 * day; //일과시간 감소
            combo = 0; //콤보 초기화
            Debug.Log("분류 실패! 장애물 있음. 일과시간 감소: " + playTime + ", 현재 콤보: " + combo + ", 최대 콤보: " + maxCombo + "점수 배율: " + scoreMag);
        }
        else // 장애물이 없을 때
        {
            if(clean) // 반려요소가 없을 때
            {
                if(confirm) // 승인 버튼 클릭 시
                {
                    success = true;
                    playTime += 1 * day; //일과시간 증가
                    combo += 1; //콤보 증가
                    feverValue += 3 * scoreMag; //피버 게이지 증가
                    score += (int)((1 * day) * scoreMag); //점수 증가
                    scoreMagnification(); //점수 배율 적용
                    if (combo > maxCombo)
                    {
                        maxCombo = combo; //최대 콤보 갱신
                    }
                    
                    Debug.Log("분류 성공! 일과시간 증가: " + playTime + ", 현재 콤보: " + combo + ", 최대 콤보: " + maxCombo + "점수 배율: " + scoreMag);
                }
                else // 반려 버튼 클릭 시
                {
                    success = false;
                    playTime -= 5 * day; //일과시간 감소
                    combo = 0; //콤보 초기화
                    scoreMagnification(); //점수 배율 적용
                    feverValue -= (float)(feverValue * 0.1); //피버 게이지 감소
                    Debug.Log("분류 실패! 일과시간 감소: " + playTime + ", 현재 콤보: " + combo + ", 최대 콤보: " + maxCombo + "점수 배율: " + scoreMag);
                }
            }
            else // 반려요소가 있을 때
            {
                if(confirm) // 승인 버튼 클릭 시
                {
                    success = false;
                    playTime -= 5 * day; //일과시간 감소
                    combo = 0; //콤보 초기화
                    scoreMagnification(); //점수 배율 적용
                    feverValue -= (float)(feverValue * 0.1); //피버 게이지 감소
                    Debug.Log("분류 실패! 반려요소 있음. 일과시간 감소: " + playTime + ", 현재 콤보: " + combo + ", 최대 콤보: " + maxCombo + "점수 배율: " + scoreMag);
                }
                else // 반려 버튼 클릭 시
                {
                    success = true;
                    playTime += 1 * day; //일과시간 증가
                    combo += 1; //콤보 증가
                    feverValue += 3 * scoreMag; //피버 게이지 증가
                    score += (int)((1 * day) * scoreMag); //점수 증가
                    scoreMagnification(); //점수 배율 적용
                    if (combo > maxCombo)
                    {
                        maxCombo = combo; //최대 콤보 갱신
                    }
                    
                    Debug.Log("분류 성공! 반려요소 없음. 일과시간 증가: " + playTime + ", 현재 콤보: " + combo + ", 최대 콤보: " + maxCombo + "점수 배율: " + scoreMag);
                }
            }
        }
    }
}
