using System;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

public class NetworkManager : Singleton<NetworkManager>
{
        [DllImport("__Internal")]
        private static extern void OpenTossLeaderboard();

        public void OnTossLeaderboard()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        OpenTossLeaderboard();
#else
                Debug.Log("Toss Leaderboard can only open in WebGL build");
#endif
        }
}