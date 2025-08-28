using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejectController : MonoBehaviour, IPoolable
{
    public GameObject OriginalPrefab { get; set; } 
    
    public Sprite _idleSprite;
    public Sprite _strokeSprite;
    
    private SpriteRenderer _sr;
    
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    public void Initialize()
    {
        if (_idleSprite != null || _strokeSprite != null) //TODO:강조 스프라이트 생기면 각 프리팹 인스펙터에 할당하고 해당 검사코드 지울것.
            _sr.sprite = _idleSprite;
    }

    public void SetStroke()
    {
        if (_idleSprite != null || _strokeSprite != null) //TODO:강조 스프라이트 생기면 각 프리팹 인스펙터에 할당하고 해당 검사코드 지울것.
            _sr.sprite = _strokeSprite;
    }
}
