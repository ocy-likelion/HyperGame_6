using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    // UI 풀에서 반환할 원본 프리팹 참조
    public GameObject OriginalPrefab { get; set; } 
    
    private DocumentController _documentController;
    private int _processCount;

    /// <summary>
    /// 초기화: DocumentController 참조와 장애물 처리 카운트 지정
    /// </summary>
    public void Initialize(DocumentController documentController, int processCount)
    {
        _documentController = documentController;
        _processCount = processCount;
        gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 장애물 클릭/터치 처리
    /// </summary>
    public void ProcessHit()
    {
        _processCount--;
        Debug.Log("Obstacle hit, remaining: " + _processCount);
        if (_processCount <= 0)
        {
            // DocumentController에 처리 완료 알림
            _documentController?.ObstacleCleared(gameObject);

            // 풀에 반환 (UI 풀에도 동일하게 적용)
            DocumentPool.Instance.ReturnObject(gameObject);
        }
    }
}
