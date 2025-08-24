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
#if UNITY_WEBGL && !UNITY_EDITOR
        try {
            LockUI(true, "Submitting...");
            SubmitTossScore(score);
        } catch (Exception e) {
            Debug.LogException(e);
            LockUI(false, "Submit failed (native)");
        }
#else
                // 에디터/브라우저 테스트용 모킹
                LockUI(true, "Submitting (mock)...");
                Invoke(nameof(MockCallback), 0.2f);
#endif
        }
        
#if !UNITY_WEBGL || UNITY_EDITOR
        void MockCallback() => OnSubmitScoreResult("MOCK_OK");
#endif
        // JS에서 호출되는 콜백
        public void OnSubmitScoreResult(string result)
        {
                if (string.IsNullOrEmpty(result)) result = "ERROR:empty";

                if (result.StartsWith("OK") || result == "MOCK_OK")
                {
                        LockUI(false, "Submit success");
                        // TODO: 성공 후 처리 (예: 리더보드 열기 버튼 활성화)
                }
                else
                {
                        Debug.LogWarning("Score submit failed: " + result);
                        LockUI(false, "Submit failed");
                        // TODO: 재시도 UI/토스트 등
                }
        }

        private void LockUI(bool locked, string msg)
        {
                var resultUI = UIManager.Instance.popupUIController.resultUIController;
                var errorImg = resultUI.errorCheckImage;

                if (errorImg) errorImg.gameObject.SetActive(locked);
                Debug.Log(msg);
        }
        
}