using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentController : MonoBehaviour
{
    //서류 프리팹
    [SerializeField] private GameObject _documentPrefab;
    //반려 요소 프리팹 (0:낙서, 1:잉크번짐, 2:커피 쏟음, 3:물쏟음, 4:인쇄불량)
    [SerializeField] private List<GameObject> _rejectObjPrefabs;
    //장애물 프리팹 (0:날벌레, 1:요구자 손, 2:포스트잇, 3:파일철, 4:서류봉투)
    [SerializeField] private List<GameObject> _obstacleObjPrefabs;
    
    //서류 및 장애물 데이터 생성
    private DocumentData _currentDocument;
    private ObstacleData _currentObstacle;
    public ObstacleData CurrentObstacle => _currentObstacle;
    
    //장애물이 치워졌는지 여부
    private bool _isClean;
    
    //서류 사이즈(반려요소 스폰지점 산출에 사용)
    private Vector3 _documentSize;
    
    //날짜 수
    private int _day;

    //테스트용 함수
    void Start()
    {
        InitDocuments();
    }
    
    public void InitDocuments()
    {
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
        
        Debug.Log(_currentDocument.documentType);
        // if 피버타임이라면
        // {
        //     _currentDocument.documentType = true
        // }
        
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
        _day = TimeController.Instance.day;
        _currentObstacle = new ObstacleData();

        _currentObstacle.processCount = Mathf.Max(1, _day / 5);
        _currentObstacle.obstacleObjIdx = Random.Range(0, _obstacleObjPrefabs.Count);

        if (_currentObstacle.obstacleObjIdx == 0 || _currentObstacle.obstacleObjIdx == 1)
        {
            _currentObstacle.spawnPosX = 1;
            _currentObstacle.spawnPosY = -3;
        }
        else if (_currentObstacle.obstacleObjIdx == 2)
        {
            float chance = Mathf.Clamp(_day * 10f, 0f, 100f);
            float roll = Random.Range(0f, 100f);
            if (roll < chance)
            {
                _currentObstacle.spawnPosX = _currentDocument.spawnPosX;
                _currentObstacle.spawnPosY = _currentDocument.spawnPosY;
            }
            else
            {
                _currentObstacle.spawnPosX = Random.Range(-_documentSize.x / 2f, _documentSize.x / 2f);
                _currentObstacle.spawnPosY = Random.Range(-_documentSize.y / 2f, _documentSize.y / 2f);
            }
        }
        
        //서류 생성 함수로
        SpawnDocument();
    }

    // 서류 생성 함수
    void SpawnDocument()
    {
        // 서류 생성
        Instantiate(_documentPrefab);

        // 서류 타입에 따라 반려 요소 생성
        if (!_currentDocument.documentType)
        {
            Vector3 rejectPos = new Vector3(_currentDocument.spawnPosX, _currentDocument.spawnPosY, 0f);
            Instantiate(_rejectObjPrefabs[_currentDocument.rejectObjIdx], rejectPos, Quaternion.identity);
        }
        
        
        //확률에 따라 장애물 생성
        float chance = Mathf.Clamp(_day * 5f, 0f, 100f);
        float roll = Random.Range(0f, 100f);
        if (roll < chance)
        {
            _isClean = false;
            Classification.Instance.obstacle = !_isClean;
            Vector3 obsPos = new Vector3(_currentObstacle.spawnPosX, _currentObstacle.spawnPosY, 0f);
            var obstacleObj = Instantiate(_obstacleObjPrefabs[_currentObstacle.obstacleObjIdx], obsPos, Quaternion.identity);
        
            // 장애물 컨트롤러 생성
            var obstacleController = obstacleObj.GetComponent<ObstacleController>();
            if (obstacleController != null)
            {
                obstacleController.Initialize(this);
            }
        }
    }
    
    //장애물이 치워지면 호출될 함수
    public void ObstacleCleared()
    {
        _isClean = true;
        Classification.Instance.obstacle = !_isClean;
    }
}
