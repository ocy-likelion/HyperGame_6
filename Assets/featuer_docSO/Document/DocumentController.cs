using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DocumentController : MonoBehaviour
{
    //서류 프리팹
    [SerializeField] private GameObject _documentPrefab;
    //반려 요소 프리팹 (0:낙서, 1:잉크번짐, 2:커피 쏟음, 3:물쏟음, 4:인쇄불량)
    [SerializeField] private List<GameObject> _rejectObjPrefabs;
    //장애물 프리팹 (0:날벌레, 1:포스트 잇, 2:요구자 손, 3:파일철, 4:서류봉투)
    [SerializeField] private List<ObstacleData> _obstacleObjDatas;
    
    //서류 및 장애물 데이터
    private DocumentData _currentDocument;
    private List<ObstacleInstance> _currentObstacles = new List<ObstacleInstance>();
    
    //생성된 서류 및 장애물 오브젝트
    private GameObject _docObj;
    private GameObject _rejectObj;
    private List<GameObject> _obstacleObjs = new List<GameObject>();
    
    //서류 사이즈(반려요소 스폰지점 산출에 사용)
    private Vector3 _documentSize;
    
    //날짜 수
    private int _day;
    
    //서류 스폰 포인트
    [SerializeField] private float _docSpawnX;
    [SerializeField] private float _docSpawnY;
    
    //서류 도착 포인트
    [SerializeField] private float _docStopPosX;
    [SerializeField] private float _docStopPosY;
    
    //서류 디스폰 포인트
    [SerializeField] private float _docDespawnX;
    [SerializeField] private float _docDespawnY;
    
    //이동 시간
    [SerializeField] private float _duration;
    
    //버튼 연타 방지용 변수
    [NonSerialized] public bool _isClickable;
    
    public void InitDocuments()
    {
        Classification.Instance.docController = this;
        _currentObstacles.Clear();
        _obstacleObjs.Clear();
        
        var renderer = _documentPrefab.GetComponent<SpriteRenderer>();
        _documentSize = renderer != null ? renderer.bounds.size : Vector3.one;
        
        CreateDocument();
    }
    
    //서류 타입 결정 함수
    void CreateDocument()
    {
        _currentDocument = new DocumentData();
        
        _currentDocument.documentType = (Random.Range(0, 2) == 0);
        Classification.Instance.clean = _currentDocument.documentType;
        
        _currentDocument.rejectObjIdx = Random.Range(0, _rejectObjPrefabs.Count);
        
        
        // todo: if 피버타임이라면
        // {
        //     _currentDocument.documentType = true
        // }
        
        // 반려 요소 크기 캐싱
        var rejectRenderer = _rejectObjPrefabs[_currentDocument.rejectObjIdx].GetComponent<SpriteRenderer>();
        
        Vector3 rejectSize = rejectRenderer != null ? rejectRenderer.bounds.size : Vector3.zero;

        float minX = -_documentSize.x / 2f + rejectSize.x / 2f;
        float maxX = _documentSize.x / 2f - rejectSize.x / 2f;
        float minY = -_documentSize.y / 2f + rejectSize.y / 2f;
        float maxY = _documentSize.y / 2f - rejectSize.y / 2f;

        _currentDocument.spawnPosX = Random.Range(minX, maxX);
        _currentDocument.spawnPosY = Random.Range(minY, maxY);
        
        
        //장애물 타입 결정 함수로
        CreateObstacle();
    }

    //장애물 타입 결정 함수
    void CreateObstacle()
    {
        _day = GameManager.Instance.GetTimeController()._day;

        int diffiycult = (_day / 5) + 1;
        int obstacleType = Random.Range(0, _obstacleObjDatas.Count);
        
        if (obstacleType == 0 || obstacleType == 1) // 날벌레, 포스트잇
        {
            for (int i = 0; i < diffiycult; i++)
            {
                var obstacle = new ObstacleInstance();
                obstacle.obstacleObjIdx = obstacleType;
                obstacle.prefab = _obstacleObjDatas[obstacleType].obstaclePrefab;
                obstacle.spawnPos = new Vector2(
                    Random.Range(-_documentSize.x / 2f, _documentSize.x / 2f),
                    Random.Range(-_documentSize.y / 2f, _documentSize.y / 2f)
                );
                obstacle.processCount = 1;
                _currentObstacles.Add(obstacle);
            }
        }
        else if (obstacleType == 2) // 손
        {
            var obstacle = new ObstacleInstance();
            obstacle.obstacleObjIdx = obstacleType;
            obstacle.prefab = _obstacleObjDatas[obstacleType].obstaclePrefab;
            obstacle.spawnPos = new Vector2(1f, -2f);   //로컬 위치 고정 (todo:추후 도장찍는 위치 결정나면 바꿔야함)
            obstacle.processCount = diffiycult;
            _currentObstacles.Add(obstacle);
        }
        else // 서류철, 폴더
        {
            var obstacle = new ObstacleInstance();
            obstacle.obstacleObjIdx = obstacleType;
            obstacle.prefab = _obstacleObjDatas[obstacleType].obstaclePrefab;
            obstacle.spawnPos = Vector2.zero;
            obstacle.processCount = diffiycult;
            _currentObstacles.Add(obstacle);
        }
        
        //서류 생성 함수로
        SpawnDocument();
    }

    // 서류 생성 함수
    void SpawnDocument()
    {
        _obstacleObjs.Clear();
        
        // 서류 생성
        _docObj = DocumentPool.Instance.GetObject(_documentPrefab, new Vector3(7, 0.5f, 0), Quaternion.identity);
        _docObj.transform.SetParent(this.transform, true);

        // 서류 타입에 따라 반려 요소 생성
        if (!_currentDocument.documentType)
        {
            _rejectObj = DocumentPool.Instance.GetObject(
                _rejectObjPrefabs[_currentDocument.rejectObjIdx], Vector3.zero, Quaternion.identity
            );
            
            // (부모: 서류 오브젝트)
            _rejectObj.transform.SetParent(_docObj.transform, false);
            _rejectObj.transform.localPosition = new Vector3(
                _currentDocument.spawnPosX, _currentDocument.spawnPosY, 0f
            );
        }
        
        
        // 확률에 따라 장애물 생성
        float chance = Mathf.Clamp(_day * 5f, 0f, 100f);
        if (Random.Range(0f, 100f) < chance)
        {
            Classification.Instance.obstacle = true;

            foreach (ObstacleInstance obstacle in _currentObstacles)
            {
                var obj = DocumentPool.Instance.GetObject(obstacle.prefab, Vector3.zero, Quaternion.identity);
                
                //(부모: 서류 오브젝트)
                obj.transform.SetParent(_docObj.transform, false);
                obj.transform.localPosition = new Vector3(
                    obstacle.spawnPos.x, 
                    obstacle.spawnPos.y, 
                    0f
                );

                _obstacleObjs.Add(obj);

                var obstacleController = obj.GetComponent<ObstacleController>();
                if (obstacleController != null)
                {
                    obstacleController.Initialize(this, obstacle.processCount);
                }
            }
        }
        
        //서류 등장 연출
        _docObj.transform.DOMove(new Vector3(_docStopPosX, _docObj.transform.position.y, _docObj.transform.position.z), _duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _docObj.transform.DOMove(new Vector3(_docObj.transform.position.x, _docStopPosY, _docObj.transform.position.z), _duration)
                    .SetEase(Ease.Linear);
            });

        _isClickable = true;
    }
    
    
    //장애물이 치워지면 호출될 함수
    public void ObstacleCleared(GameObject obstacleObj)
    {
        if (_obstacleObjs.Contains(obstacleObj))
        {
            _obstacleObjs.Remove(obstacleObj);
        }

        if (_obstacleObjs.Count == 0)
        {
            Classification.Instance.obstacle = false;
        }
    }
    
    // 서류 치우기 함수
    public void RemoveDocument()
    {
        _isClickable = false;
        
        //서류 퇴장 연출
        _docObj.transform.DOMove(new Vector3(_docObj.transform.position.x, _docDespawnY, _docObj.transform.position.z), _duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _docObj.transform.DOMove(new Vector3(_docDespawnX, _docObj.transform.position.y, _docObj.transform.position.z), _duration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => ReloadDocument());
            });
    }

    public void ReloadDocument(bool noLoop = false)
    {
        if (_docObj != null)
        {
            // 자식 오브젝트들을 먼저 풀에 반환
            for (int i = _docObj.transform.childCount - 1; i >= 0; i--)
            {
                var child = _docObj.transform.GetChild(i).gameObject;
                DocumentPool.Instance.ReturnObject(child);
            }

            // 마지막에 서류 자체 반환
            DocumentPool.Instance.ReturnObject(_docObj);
        }

        _currentObstacles.Clear();
        _obstacleObjs.Clear();
        _rejectObj = null;
        _docObj = null;
        Classification.Instance.obstacle = false;

        if(!noLoop) CreateDocument();
    }
    void Update()
    {
        if (IsInputDown(out Vector2 inputPos))
        {
            RaycastHit2D hit = Physics2D.Raycast(inputPos, Vector2.zero);

            if (hit.collider != null)
            {
                ObstacleController obstacle = hit.collider.GetComponent<ObstacleController>();
                if (obstacle != null)
                {
                    obstacle.ProcessHit();
                }
            }
        }
    }

    private bool IsInputDown(out Vector2 inputPos)
    {
        inputPos = Vector2.zero;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return true;
        }
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            inputPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            return true;
        }
#endif
        return false;
    }

}
