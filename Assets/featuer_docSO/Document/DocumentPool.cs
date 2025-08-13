using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentPool : Singleton<DocumentPool>
{
    // 프리팹 별로 큐를 관리하는 딕셔너리
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    // 오브젝트 요청 메서드
    // prefab: 생성/재사용할 오브젝트의 원본 프리팹
    // position: 배치할 위치
    // rotation: 배치할 회전값
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // 해당 프리팹의 큐가 없으면 새로 생성
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }

        GameObject obj;
        
        // 큐가 비어있으면 새로 Instantiate, 아니면 큐에서 꺼내서 재사용
        if (poolDictionary[prefab].Count == 0)
        {
            obj = Instantiate(prefab);
        }
        else
        {
            obj = poolDictionary[prefab].Dequeue();
        }

        // 오리지널 프리팹 정보 저장 (되돌릴 때 사용)
        var obstacleController = obj.GetComponent<ObstacleController>();
        if (obstacleController != null)
        {
            obstacleController.OriginalPrefab = prefab;
        }

        var rejectController = obj.GetComponent<RejectController>();
        if (rejectController != null)
        {
            rejectController.OriginalPrefab = prefab;
        }
        
        var document = obj.GetComponent<Document>();
        if (document != null)
        {
            document.OriginalPrefab = prefab;
        }
        //
        
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        
        
        obj.SetActive(true);
        Debug.Log("GetObject: " + prefab.name + ", Active: " + obj.activeSelf);

        return obj;
    }

    // 오브젝트 반환 메서드 (비활성화 후 풀에 넣기)
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);

        // 오브젝트에 붙어 있는 스크립트에서 원래의 프리팹 정보 가져오기
        var obstacleController = obj.GetComponent<ObstacleController>();
        var rejectController = obj.GetComponent<RejectController>();
        var document = obj.GetComponent<Document>();

        GameObject prefabKey = null;

        if (obstacleController != null)
            prefabKey = obstacleController.OriginalPrefab;
        else if (rejectController != null)
            prefabKey = rejectController.OriginalPrefab;
        else if (document != null)
            prefabKey = document.OriginalPrefab;
        
        // 프리팹 정보를 못 찾으면 경고 출력 후 오브젝트 삭제
        if (prefabKey == null)
        {
            Debug.LogWarning("ReturnObject: OriginalPrefab not found on object, destroying.");
            Destroy(obj);
            return;
        }

        // 해당 프리팹의 큐가 없으면 새로 생성
        if (!poolDictionary.ContainsKey(prefabKey))
            poolDictionary[prefabKey] = new Queue<GameObject>();

        poolDictionary[prefabKey].Enqueue(obj);
    }

}
