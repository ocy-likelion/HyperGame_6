using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
/*
**사용법**
VFXManager는 VFXType 클래스의 상수 참조로 구성됩니다.
먼저 project탭에서 Datas/ScriptableObject/VfxDatas 폴더경로를 찾아주세요.
거기서 우클릭을 하고 Create/SO/FX/VFXData를 찾아 선택하면 SO파일이 하나 생성됩니다.
등록할 프리팹과 함께 상세정보(지속시간, 풀링해둘 갯수) 기입해주시고
*프리팹은 EditCanvas를 통해 생성해주세요. VFX는 UI로 취급됩니다.* (VFXDataSO 스크립트 참조)
*가장 중요* 사용할 파일명을 목적에 맞게 지어주세요.
 그리고 같은경로내에 있는 VFXList라는 SO파일을 클릭하면 항목을 추가하고 방금생성한 SO를 넣어주신 후,
이 파일명을 그대로 VFXType 클래스의 string 상수로 작성(등록)해주시면 준비가 끝납니다.
(상수 등록은 VFXType 클래스의 설명을 참조해주세요.)

사용하실때는 VfxManager.Instance.GetVFX(VFXType.등록한상수, 생성할 위치 , 각도, 크기(옵션)); 이렇게 하면 됩니다.
아래는 예시입니다.
VfxManager.Instance.GetVFX(VFXType.TEST, new Vector2(0,0) , Quaternion.identity, Vector2.one);
*/

/// <summary>
/// VFX데이터들을 등록하는 클래스 입니다. 값은 반드시 등록한 VFXDataSO의 파일명과 동일하게 작성해주세요.
/// 호출때 사용할 변수명은 편하게 지으시면 됩니다.
/// </summary>
public static class VFXType
{
    //새 VFX 등록 시 public const string '호출때 쓸 변수명' = "생성한 VfxDataSO의 파일명" 이렇게 작성해주세요.
    //public const string FireWork = "Firreeee";
    public const string TEST = "TestVfx";
}

/// <summary>
/// Vfx 관리자입니다. Vfx를 미리 생성/풀링하고 필요할때마다 꺼내쓸 수 있습니다.
/// </summary>
public class VfxManager : Singleton<VfxManager>
{
    Dictionary<string, Queue<GameObject>> vfxPools = new Dictionary<string, Queue<GameObject>>();
    Dictionary<string, VFXDataSO> vfxDataSOs = new Dictionary<string, VFXDataSO>();

    [SerializeField] private VFXListSO vfxListSO;
    
    protected override void Initialize()
    {
        if(vfxListSO != null) LoadVFX(vfxListSO);
    }

    private void LoadVFX(VFXListSO vfxListSO)
    {
        foreach(var vfxDataSo  in vfxListSO.VFXList)
        {
            Instance.RegisterVFX(vfxDataSo);
        }
    }
    
    private void RegisterVFX(VFXDataSO vfxDataSO)
    {
        if (!vfxPools.ContainsKey(vfxDataSO.name))
        {
            vfxPools[vfxDataSO.name] = new Queue<GameObject>();
            vfxDataSOs[vfxDataSO.name] = vfxDataSO;

            for (int i = 0; i < vfxDataSO.poolSize; i++)
            {
                GameObject vfxObject = Instantiate(vfxDataSO.vfxPrefab, transform);
                vfxObject.SetActive(false);
                vfxPools[vfxDataSO.name].Enqueue(vfxObject);
            }
        }
    }
    
    private GameObject DequeueVFX(string vfxType)
    {
        if (!vfxPools.ContainsKey(vfxType))
        {
            Debug.LogError($"{vfxType} doesn't exist in VFXPools!");
            return null;
        }
        
        if (vfxPools[vfxType].Count <= 0)
        {
            return Instantiate(vfxDataSOs[vfxType].vfxPrefab, Instance.transform);
        }
        else
        {
            return vfxPools[vfxType].Dequeue();
        }
    }
    
    public void ReturnVFX(string vfxType, GameObject vfxObject)
    {
        vfxObject.transform.SetParent(Instance.transform);
        vfxObject.SetActive(false);
        vfxPools[vfxType].Enqueue(vfxObject);
    }

    public GameObject GetVFX(string vfxType, Vector2 position, Quaternion rotation, Vector2 size = default, bool returnAutomatically = true)
    {
        GameObject vfxObject = DequeueVFX(vfxType);
        RectTransform vfxRectTransform = vfxObject.transform as RectTransform;

        if (vfxRectTransform != null)
        {
            vfxRectTransform.anchoredPosition = position;
            vfxRectTransform.rotation = rotation;
            vfxRectTransform.localScale = size;
        }
        vfxObject.SetActive(true);

        if (returnAutomatically)
        {
            StartCoroutine(AutoMatic(ReturnVFX(vfxType, vfxObject, vfxDataSOs[vfxType].duration)));
        }
        return vfxObject;
    }
    
    private IEnumerator ReturnVFX(string vfxType, GameObject vfxObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        vfxObject.transform.SetParent(Instance.transform);
        vfxObject.SetActive(false);
        vfxPools[vfxType].Enqueue(vfxObject);
    }

    private static IEnumerator AutoMatic(IEnumerator callback)
    {
        yield return callback;
    }
    
}
