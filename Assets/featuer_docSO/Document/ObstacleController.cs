using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleController : MonoBehaviour, IPoolable
{
    // UI 풀에서 반환할 원본 프리팹 참조
    public GameObject OriginalPrefab { get; set; }
    public Image obstacleImage;
    
    private DocumentController _documentController;
    private int _processCount;
    private int _obstacleObjIdx;
    private bool _processOver;

    /// <summary>
    /// 초기화: DocumentController 참조와 장애물 처리 카운트 지정
    /// </summary>
    public void Initialize(DocumentController documentController, int processCount, int obstacleObjIdx)
    {
        _documentController = documentController;
        _processCount = processCount;
        _obstacleObjIdx = obstacleObjIdx;
        _processOver = false;
        
        gameObject.SetActive(true);

        StartCoroutine(GameManager.Instance.obstacleClearEffect.IdleAnim(this, _obstacleObjIdx));
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
            //마지막 처리모션 한번만 허용되도록 수정
            if (_processOver) return;
            _processOver = true;
            StartCoroutine(TerminateSeq());

        }
        else
        {
            //연속처리 필요대상만 실행됨
            StopCoroutine(GameManager.Instance.obstacleClearEffect.HitAnim(this, _obstacleObjIdx));
            StartCoroutine(GameManager.Instance.obstacleClearEffect.HitAnim(this, _obstacleObjIdx));
        }
    }

    public int GetProcessCount()
    {
        return _processCount;
    }

    public IEnumerator TerminateSeq()
    {
        // DocumentController에 처리 완료 알림
        _documentController?.ObstacleCleared(gameObject);
        
        //장애물 처리 연출
        yield return StartCoroutine(GameManager.Instance.
            obstacleClearEffect.DefuseEffect(this, _obstacleObjIdx));

        // 풀에 반환 (UI 풀에도 동일하게 적용)
        DocumentPool.Instance.ReturnObject(gameObject);
    }
}