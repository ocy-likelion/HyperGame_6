using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentPool : Singleton<DocumentPool>
{
    // 프리팹 별로 큐 관리
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    [NonSerialized] public Canvas canvas; // UI 풀이 붙을 Canvas

    /// <summary>
    /// UI 오브젝트 요청
    /// </summary>
    /// <param name="prefab">생성/재사용할 UI 프리팹</param>
    /// <param name="anchoredPosition">Canvas 기준 위치</param>
    public GameObject GetObject(GameObject prefab, Vector2 anchoredPosition)
    {
        if (!poolDictionary.ContainsKey(prefab))
            poolDictionary[prefab] = new Queue<GameObject>();

        GameObject obj = poolDictionary[prefab].Count > 0 ? poolDictionary[prefab].Dequeue() : Instantiate(prefab);

        // OriginalPrefab 저장
        var obstacle = obj.GetComponent<ObstacleController>();
        if (obstacle != null) obstacle.OriginalPrefab = prefab;

        var reject = obj.GetComponent<RejectController>();
        if (reject != null) reject.OriginalPrefab = prefab;

        var document = obj.GetComponent<Document>();
        if (document != null) document.OriginalPrefab = prefab;

        // RectTransform 기준 배치
        var rect = obj.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.SetParent(canvas.transform, false);
            rect.anchoredPosition = anchoredPosition;
            rect.localRotation = Quaternion.identity;
        }
        else
        {
            obj.transform.position = anchoredPosition;
            obj.transform.rotation = Quaternion.identity;
        }

        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// UI 오브젝트 반환
    /// </summary>
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);

        var obstacle = obj.GetComponent<ObstacleController>();
        var reject = obj.GetComponent<RejectController>();
        var document = obj.GetComponent<Document>();

        GameObject prefabKey = obstacle?.OriginalPrefab ?? reject?.OriginalPrefab ?? document?.OriginalPrefab;

        if (prefabKey == null)
        {
            Debug.LogWarning("ReturnObject: OriginalPrefab not found, destroying object.");
            Destroy(obj);
            return;
        }

        var rect = obj.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.SetParent(canvas.transform, false);
            rect.anchoredPosition = Vector2.zero;
            rect.localRotation = Quaternion.identity;
        }

        if (!poolDictionary.ContainsKey(prefabKey))
            poolDictionary[prefabKey] = new Queue<GameObject>();

        poolDictionary[prefabKey].Enqueue(obj);
    }

    /// <summary>
    /// 풀 전체 초기화
    /// </summary>
    public void ClearPool()
    {
        foreach (var queue in poolDictionary.Values)
        {
            while (queue.Count > 0)
            {
                GameObject obj = queue.Dequeue();
                if (obj != null)
                    Destroy(obj);
            }
        }

        poolDictionary.Clear();
        Debug.Log("DocumentPool cleared.");
    }
}
