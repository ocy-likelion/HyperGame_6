using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject OriginalPrefab { get; set; } 
    
    private DocumentController _documentController;
    private int _processCount;

    // 의존성 주입용 초기화 함수
    public void Initialize(DocumentController documentController, int processCount)
    {
        _documentController = documentController;
        _processCount = processCount;
        gameObject.SetActive(true);
    }
    
    public void ProcessHit()
    {
        _processCount--;
        Debug.Log(_processCount);
        if (_processCount <= 0)
        {
            _documentController?.ObstacleCleared(gameObject);
            DocumentPool.Instance.ReturnObject(gameObject);
        }
    }
}
