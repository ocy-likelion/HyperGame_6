using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//씬 전환을 제어합니다.
public class SceneController : Singleton<SceneController>
{
    private bool _sceneReady = false;
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _sceneReady = true;
    }

    /// <summary>
    /// 씬을 전환합니다.
    /// </summary>
    /// <param name="sceneName">로드할 씬의 SceneState를 넣어주세요</param>
    /// <param name="sceneLoadedAction">(옵션)씬 로드 후, 실행할 콜백을 넣어주세요.</param>
    public static void TransitionToScene(SceneState sceneName, Func<IEnumerator> sceneLoadedAction = null, Func<IEnumerator> onCompleted = null)
    {
        Instance.StartCoroutine(Instance.TransitionScene(sceneName, sceneLoadedAction: sceneLoadedAction, onCompleted: onCompleted));
    }
    
    protected IEnumerator TransitionScene(SceneState newSceneIndex,float loadingDelay = 1f, bool isStopTimeScale = false, Func<IEnumerator> sceneLoadedAction = null, Func<IEnumerator> onCompleted = null)
    {
        //호출하는 씬이 같은 씬이면 진행 중단.
        if ((int)newSceneIndex == SceneManager.GetSceneByBuildIndex(0).buildIndex)
        {
            Debug.LogWarning("Scene " + newSceneIndex + " is already loaded");
            yield break;
        }
        
        //씬 호출.
        if (isStopTimeScale)
            Time.timeScale = 0f;
            
        _sceneReady = false;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        yield return SceneManager.LoadSceneAsync((int)newSceneIndex);

        yield return new WaitUntil(() => _sceneReady);
        yield return new WaitForSecondsRealtime(loadingDelay);
        
        //콜백이 있으면 실행 후 완료까지 대기.
        if (sceneLoadedAction != null)
        {
            yield return StartCoroutine(sceneLoadedAction());
        }

        if (isStopTimeScale)
            Time.timeScale = 1f;
        
        //씬로드 완료 후 실행
        onCompleted?.Invoke();
    }
}
