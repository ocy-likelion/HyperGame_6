using System;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

public class NetworkManager : Singleton<NetworkManager>
{
        [DllImport("__Internal")]
        private static extern void OpenTossLeaderboard();
        private static extern void SubmitTossScore(int score);

        public void OnTossLeaderboard()
        {
#if UNITY_WEBGL && !UNITY_EDITOR//WebGL 환경에서만 동작
        OpenTossLeaderboard();
#else
                Debug.Log("Toss Leaderboard can only open in WebGL build");
#endif
        }
        
        public void SendScore(int score)
        {
#if UNITY_WEBGL && !UNITY_EDITOR//WebGL 환경에서만 동작
        SubmitTossScore(score);
#else
                Debug.Log($"[Editor] 점수 전송 시도: {score}");
#endif
        }
}