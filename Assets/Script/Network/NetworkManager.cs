using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;


public class NetworkManager : Singleton<NetworkManager>
{
        //Vite로 점수 보내기
        IEnumerator PostScoreToVite(GameInfo gameInfo, Action success, Action failure)
        {
                //gameInfo = new GameInfo { score = 123 };
                string jsonString = JsonConvert.SerializeObject(gameInfo);

                using (UnityWebRequest req = 
                       new UnityWebRequest(
                               Constants.ViteURL+"/api/echo", 
                               UnityWebRequest.kHttpVerbPOST))
                {
                        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
                        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
                        req.downloadHandler = new DownloadHandlerBuffer();
                        req.SetRequestHeader("Content-Type", "application/json");

                        yield return req.SendWebRequest();

                        if (req.result == UnityWebRequest.Result.Success)
                                Debug.Log("Response: " + req.downloadHandler.text);
                        else
                                Debug.LogError("Error: " + req.error);
                        switch (req.result)
                        {
                                case UnityWebRequest.Result.Success:
                                        Debug.Log("Response: " + req.downloadHandler.text);
                                        break;
                                case UnityWebRequest.Result.ConnectionError:
                                case UnityWebRequest.Result.ProtocolError:
                                {
                                        if (req.responseCode == 403)
                                        {
                                                Debug.Log("로그인이 필요합니다.");
                                        }
                
                                        failure?.Invoke();
                                        break;
                                }
                                default:
                                        // var result = www.downloadHandler.text;
                                        // var user = JsonUtility.FromJson<UserInfo>(result);
                                        success?.Invoke();
                                        break;
                        }
                }
        }
        
        public void SendScore(GameInfo gameInfo, Action success, Action failure)
        {
                StartCoroutine(PostScoreToVite(gameInfo, success, failure));
        }
        
        //Vite로부터 점수 받기
        IEnumerator GetScoreFromVite(Action<GameInfo> success, Action failure)
        {
                using (UnityWebRequest req = 
                       new UnityWebRequest(
                               Constants.ViteURL+"/api/score", 
                               UnityWebRequest.kHttpVerbGET))
                {
                        req.downloadHandler = new DownloadHandlerBuffer();

                        yield return req.SendWebRequest();

                        if (req.result == UnityWebRequest.Result.ConnectionError ||
                            req.result == UnityWebRequest.Result.ProtocolError)
                        {
                                if (req.responseCode == 403)
                                {
                                        Debug.Log("로그인이 필요합니다.");
                                }
                
                                failure?.Invoke();
                        }
                        else
                        {
                                var result = req.downloadHandler.text;
                                var userInfos = JsonConvert.DeserializeObject<GameInfo>(result);
                
                                success?.Invoke(userInfos);
                        }
                }
        }
        
        public void RecieveScore(Action<GameInfo> success, Action failure)
        {
                StartCoroutine(GetScoreFromVite(success, failure));
        }
}