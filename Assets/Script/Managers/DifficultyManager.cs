using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : Singleton<DifficultyManager>
{
    // 하루 시간 (게임 내 일과 시간 총량)
    private int _dayTime = 10;
    public int DayTime => _dayTime;
    
    // 난이도 상승 주기 (예: n일마다 난이도 증가)
    private int _difficultyIncreaseInterval = 5;
    
    // 난이도별 시간 감소 주기 (초당 1씩감소)
    private float[] _timeDecreaseRates = { 1f, 0.6f, 0.2f };
    
    // 서류 처리 보상 (난이도별 일과 시간 회복량)
    private int[] _reward = { 1, 3, 5 };
    
    // 서류 처리 실수 패널티 (난이도별 일과 시간 손실량)
    private int[] _penalty = { 3, 7, 12 };

    // 장애물 등장 확률 (단위: 퍼센트)
    private int[] _ObstacleSpawnProbability = { 5, 20, 40 };

    
    //현재 day에 맞춰 난이도를 산출하는 메서드
    public int GetLevel(int day)
    {
        return day / _difficultyIncreaseInterval;
    }
    
    //현재 day에 맞춰 난이도별 시간 감소 주기를 반환하는 메서드
    public float GetTimeDecreaseRate(int day)
    {
        int level = GetLevel(day);
        level = Mathf.Min(level, _timeDecreaseRates.Length - 1);
        return _timeDecreaseRates[level];
    }
    
    //현재 day에 맞춰 서류 처리 보상을 반환하는 메서드
    public int GetReward(int day)
    {
        int level = GetLevel(day);
        level = Mathf.Min(level, _reward.Length - 1);
        return _reward[level];
    }
    
    //현재 day에 맞춰 서류 처리 실수 패널티를 반환하는 메서드
    public int GetPenalty(int day)
    {
        int level = GetLevel(day);
        level = Mathf.Min(level, _penalty.Length - 1);
        return _penalty[level];
    }

    //현재 day에 맞춰 장애물 등장 확률을 반환하는 메서드
    public int GetObstacleSpawnProbability(int day)
    {
        int level = GetLevel(day);
        level = Mathf.Min(level, _ObstacleSpawnProbability.Length - 1);
        return _ObstacleSpawnProbability[level];
    }
}
