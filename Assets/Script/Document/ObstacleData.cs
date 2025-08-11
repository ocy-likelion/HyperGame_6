using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleData : ScriptableObject
{
    public Sprite obstacleSprite;
    
    // 장애물 인덱스
    public int obstacleObjIdx;
    
    // 장애물 체력
    public int processCount;
    
    // 장애물 스폰 포지션
    public float spawnPosX;
    public float spawnPosY;
}
