using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        Debug.Log("서류타입: " + _currentDocument.documentType);
        var rejectRenderer = _rejectObjPrefabs[_currentDocument.rejectObjIdx].GetComponent<SpriteRenderer>();
        
        //반려요소 위치 랜덤 생성
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
            obstacle.spawnPos = new Vector2(1f, -3f);
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
        _docObj = DocumentPool.Instance.GetObject(_documentPrefab, Vector3.zero, Quaternion.identity);

        // 서류 타입에 따라 반려 요소 생성
        if (!_currentDocument.documentType)
        {
            Vector3 rejectPos = new Vector3(_currentDocument.spawnPosX, _currentDocument.spawnPosY, 0f);
            _rejectObj = DocumentPool.Instance.GetObject(_rejectObjPrefabs[_currentDocument.rejectObjIdx], rejectPos, Quaternion.identity);
        }
        
        
        //확률에 따라 장애물 생성
        float chance = Mathf.Clamp(_day * 5f, 0f, 100f);
        if (Random.Range(0f, 100f) < chance) //todo: 피버구현 후 조건에 'AND 피버타임이 아닐 경우'를 추가해야함
        {
            Classification.Instance.obstacle = true;
            
            foreach (ObstacleInstance obstacle in _currentObstacles)
            {
                Vector3 obsPos = new Vector3(obstacle.spawnPos.x, obstacle.spawnPos.y, 0f);
                var obj = DocumentPool.Instance.GetObject(obstacle.prefab, obsPos, Quaternion.identity);
                _obstacleObjs.Add(obj);

                var obstacleController = obj.GetComponent<ObstacleController>();
                if (obstacleController != null)
                {
                    obstacleController.Initialize(this, obstacle.processCount);
                }
            }
        }
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
    public void ReloadDocument()
    {
        DocumentPool.Instance.ReturnObject(_docObj);

        if (_rejectObj != null)
        {
            DocumentPool.Instance.ReturnObject(_rejectObj);
        }

        foreach (var obj in _obstacleObjs)
        {
            DocumentPool.Instance.ReturnObject(obj);
        }
        
        _currentObstacles.Clear();
        _obstacleObjs.Clear();
        
        CreateDocument();
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
