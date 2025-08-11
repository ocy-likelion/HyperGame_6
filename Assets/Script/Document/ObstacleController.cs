using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private DocumentController _documentController;
    private int _processCount;

    // 의존성 주입용 초기화 함수
    public void Initialize(DocumentController documentController)
    {
        _documentController = documentController;
        _processCount = documentController.CurrentObstacle.processCount;
    }

    private bool _isInputDown()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButtonDown(0);
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL
        return Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began;
#else
        return false;
#endif
    }
    void Update()
    {
        if (_isInputDown())
        {
            Vector2 inputPos;
#if UNITY_EDITOR || UNITY_STANDALONE
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL
            inputPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
#endif
            RaycastHit2D hit = Physics2D.Raycast(inputPos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                _processCount--;
                Debug.Log(_processCount);
                if (_processCount <= 0)
                {
                    // DocumentController 쪽 상태 변경
                    _documentController?.ObstacleCleared();

                    Destroy(gameObject);
                }
            }
        }
    }
}
