using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DocumentController : MonoBehaviour
{
    [Header("프리팹")]
    [SerializeField] private GameObject _documentPrefab;
    [SerializeField] private List<GameObject> _rejectObjPrefabs;
    [SerializeField] private List<ObstacleData> _obstacleObjDatas;

    [Header("도장")]
    [SerializeField] private GameObject approvalStampPrefab;
    [SerializeField] private GameObject deniedStampPrefab;

    [Header("위치 및 이동")]
    [SerializeField] private Vector2 _docSpawnPos;
    [SerializeField] private Vector2 _docStopPos;
    [SerializeField] private Vector2 _docDespawnPos;
    [SerializeField] private float _duration;

    private DocumentData _currentDocument;
    private List<ObstacleInstance> _currentObstacles = new List<ObstacleInstance>();
    private List<GameObject> _obstacleObjs = new List<GameObject>();

    private GameObject _docObj;
    private GameObject _rejectObj;

    private Vector2 _documentSize;

    [NonSerialized] public bool _isClickable;

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    public void InitDocuments()
    {
        _currentObstacles.Clear();
        _obstacleObjs.Clear();
        
        DocumentPool.Instance.canvas = _canvas;

        var image = _documentPrefab.GetComponent<Image>();
        _documentSize = image != null ? image.rectTransform.rect.size : Vector2.one;

        CreateDocument();
    }

    private void CreateDocument()
    {
        _currentDocument = new DocumentData
        {
            documentType = GameManager.Instance.GetClassification().fever || (Random.Range(0, 2) == 0),
            rejectObjIdx = Random.Range(0, _rejectObjPrefabs.Count)
        };
        GameManager.Instance.GetClassification().clean = _currentDocument.documentType;

        // 반려 요소 스폰 위치 계산
        var rejectRect = _rejectObjPrefabs[_currentDocument.rejectObjIdx].GetComponent<RectTransform>();
        Vector2 rejectSize = rejectRect != null ? rejectRect.rect.size : Vector2.zero;

        float minX = -_documentSize.x / 2f + rejectSize.x / 2f;
        float maxX = _documentSize.x / 2f - rejectSize.x / 2f;
        float minY = -_documentSize.y / 2f + rejectSize.y / 2f;
        float maxY = _documentSize.y / 2f - rejectSize.y / 2f;

        _currentDocument.spawnPosX = Random.Range(minX, maxX);
        _currentDocument.spawnPosY = Random.Range(minY, maxY);

        CreateObstacle();
    }

    private void CreateObstacle()
    {
        int day = GameManager.Instance.GetTimeController()._day;
        int difficulty = (day / 5) + 1;
        int obstacleType = Random.Range(0, _obstacleObjDatas.Count);

        // 장애물 인스턴스 생성
        _currentObstacles.Clear();

        if (obstacleType == 0 || obstacleType == 1)
        {
            for (int i = 0; i < difficulty; i++)
            {
                var obstacle = new ObstacleInstance
                {
                    obstacleObjIdx = obstacleType,
                    prefab = _obstacleObjDatas[obstacleType].obstaclePrefab,
                    processCount = 1,
                    spawnPos = new Vector2(
                        Random.Range(-_documentSize.x / 2f, _documentSize.x / 2f),
                        Random.Range(-_documentSize.y / 2f, _documentSize.y / 2f)
                    )
                };
                _currentObstacles.Add(obstacle);
            }
        }
        else if (obstacleType == 2)
        {
            var obstacle = new ObstacleInstance
            {
                obstacleObjIdx = obstacleType,
                prefab = _obstacleObjDatas[obstacleType].obstaclePrefab,
                processCount = difficulty,
                spawnPos = new Vector2(1f, -2f)
            };
            _currentObstacles.Add(obstacle);
        }
        else
        {
            var obstacle = new ObstacleInstance
            {
                obstacleObjIdx = obstacleType,
                prefab = _obstacleObjDatas[obstacleType].obstaclePrefab,
                processCount = difficulty,
                spawnPos = Vector2.zero
            };
            _currentObstacles.Add(obstacle);
        }

        SpawnDocument();
    }

    private void SpawnDocument()
    {
        _obstacleObjs.Clear();

        // 문서 생성
        _docObj = DocumentPool.Instance.GetObject(_documentPrefab, _docSpawnPos);
        _docObj.GetComponent<RectTransform>().SetParent(this.GetComponent<RectTransform>(), false);

        // 반려 요소 생성
        if (!_currentDocument.documentType)
        {
            _rejectObj = DocumentPool.Instance.GetObject(
                _rejectObjPrefabs[_currentDocument.rejectObjIdx],
                new Vector2(_currentDocument.spawnPosX, _currentDocument.spawnPosY)
            );
            _rejectObj.GetComponent<RectTransform>().SetParent(_docObj.GetComponent<RectTransform>(), false);
        }

        // 장애물 생성
        float chance = Mathf.Clamp(GameManager.Instance.GetTimeController()._day * 5f, 0f, 100f);
        if (!GameManager.Instance.GetClassification().fever && Random.Range(0f, 100f) < chance)
        {
            GameManager.Instance.GetClassification().obstacle = true;

            foreach (var obstacle in _currentObstacles)
            {
                var obj = DocumentPool.Instance.GetObject(obstacle.prefab, obstacle.spawnPos);
                obj.GetComponent<RectTransform>().SetParent(_docObj.GetComponent<RectTransform>(), false);
                _obstacleObjs.Add(obj);

                var controller = obj.GetComponent<ObstacleController>();
                if (controller != null) controller.Initialize(this, obstacle.processCount);
            }
        }
        
        var docRect = _docObj.GetComponent<RectTransform>();
        // 등장 연출
        docRect.DOAnchorPosX(_docStopPos.x, _duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // X 이동 완료 후 Y 이동
                docRect.DOAnchorPosY(_docStopPos.y, _duration)
                    .SetEase(Ease.Linear);
            });
        _isClickable = true;
    }

    public void ShowStamp(bool isApproved)
    {
        if (_docObj == null) return;

        GameObject prefab = isApproved ? approvalStampPrefab : deniedStampPrefab;
        GameObject stamp = Instantiate(prefab, _docObj.transform, false);
        stamp.GetComponent<RectTransform>().anchoredPosition = new Vector2(1f, -2f);
    }

    public void ObstacleCleared(GameObject obstacleObj)
    {
        if (_obstacleObjs.Contains(obstacleObj))
            _obstacleObjs.Remove(obstacleObj);

        if (_obstacleObjs.Count == 0)
            GameManager.Instance.GetClassification().obstacle = false;
    }

    public void RemoveDocument()
    {
        _isClickable = false;
        var docRect = _docObj.GetComponent<RectTransform>();

        // Y 먼저 이동
        docRect.DOAnchorPosY(_docDespawnPos.y, _duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // Y 이동 완료 후 X 이동
                docRect.DOAnchorPosX(_docDespawnPos.x, _duration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        // X 이동 완료 후 ReloadDocument 호출
                        ReloadDocument();
                    });
            });
    }

    public void ReloadDocument(bool noLoop = false)
    {
        if (_docObj != null)
        {
            for (int i = _docObj.transform.childCount - 1; i >= 0; i--)
            {
                var child = _docObj.transform.GetChild(i).gameObject;
                if (child.CompareTag("Stamp"))
                    Destroy(child);
                else
                    DocumentPool.Instance.ReturnObject(child);
            }
            DocumentPool.Instance.ReturnObject(_docObj);
        }

        _currentObstacles.Clear();
        _obstacleObjs.Clear();
        _rejectObj = null;
        _docObj = null;
        GameManager.Instance.GetClassification().obstacle = false;

        if (!noLoop) CreateDocument();
    }

    private void Update()
    {
        if (!_isClickable) return;

        if (TryGetInputPosition(out Vector2 inputPos))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = inputPos;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                var obstacle = result.gameObject.GetComponent<ObstacleController>();
                if (obstacle != null)
                {
                    // 스크린 좌표를 Canvas 좌표로 변환
                    Vector2 canvasPos = ScreenToCanvasPosition(inputPos);
                    VfxManager.Instance.GetVFX(VFXType.OBSTOUCH, canvasPos, Quaternion.identity, Vector2.one);
                    
                    obstacle.ProcessHit();
                    return;
                }
            }
        }
    }
    
    private Vector2 ScreenToCanvasPosition(Vector2 screenPos)
    {
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screenPos,
            _canvas.worldCamera,
            out canvasPos
        );
    
        return canvasPos;
    }

    private bool TryGetInputPosition(out Vector2 inputPos)
    {
        inputPos = Vector2.zero;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                inputPos = touch.position;
                return true;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            inputPos = Input.mousePosition;
            return true;
        }

        return false;
    }
}
