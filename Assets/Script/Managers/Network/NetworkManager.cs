using System;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

public class NetworkManager : Singleton<NetworkManager>
{
        [DllImport("__Internal")]
        private static extern void OpenTossLeaderboard();
        
        [DllImport("__Internal")]
        private static extern void SubmitTossScore(int score);
        
        [DllImport("__Internal")]
        private static extern void LoadInterstitialAd();
        [DllImport("__Internal")]
        private static extern void ShowInterstitialAd();

        protected override void Initialize()
        {
                LoadAd();
        }
        
        /// <summary>
        /// 토스 리더보드 호출
        /// </summary>
        public void OnTossLeaderboard()
        {
#if UNITY_WEBGL && !UNITY_EDITOR//WebGL 환경에서만 동작
                OpenTossLeaderboard();
                
#else
                Debug.Log("Toss Leaderboard can only open in WebGL build");
#endif
        }
        
        /// <summary>
        /// 토스 리더보드에 점수 전송
        /// </summary>
        /// <param name="score"></param>
        public void SendScore(int score)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        try {
            SubmitTossScore(score);
            LockUI(true, "Submitting...");
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
                        LockUI(true, "Submit success");
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

                if (errorImg) errorImg.gameObject.SetActive(!locked);
                Debug.Log(msg);
        }
        
        
        // 게임 종료 시 호출
        public void ShowAd()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        try {
                ShowInterstitialAd();
                UIManager.Instance.popupUIController.ShowAdBg();
        } catch (Exception e) {
                Debug.LogException(e);
                //Toss가 아닌 WebGL환경에선 로직이 멈춰버리므로 추가함.
                GameManager.Instance.inGameController.EndAdMob();
        }
#else
                Debug.Log("[Editor] Show interstitial ad (mock).");
                
                //에디터에선 광고가 나오지 않으므로 패스시키기.
                GameManager.Instance.inGameController.EndAdMob();
                UIManager.Instance.popupUIController.HideAdBg();
#endif
        }
        
        public void LoadAd()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
       try {
                LoadInterstitialAd();  // 게임 시작 시 미리 로드
        } catch (Exception e) {
                Debug.LogException(e);
                Debug.Log("[Editor] Load interstitial ad (mock).");
        }
#else
                Debug.Log("[Editor] Load interstitial ad (mock).");
#endif
        }
        
        public void OnAdEvent(string eventType)
        {
                Debug.Log($"광고 이벤트 수신: {eventType}");

                //디버깅용
                if (UIManager.Instance.popupUIController.AdmobDebugText.gameObject.activeSelf)
                {
                        UIManager.Instance.popupUIController.SetAdmobDebugText(eventType);
                }

                //송신받은 event메시지에 따라 다른 액션
                switch (eventType)
                {
                        //LoadAd쪽
                        case "loaded":
                                Debug.Log("광고 로드 성공");
                                break;
                        case "clicked":
                                Debug.Log("광고 클릭");
                                break;
                        case "dismissed":
                                Debug.Log("광고 닫힘");
                                EndAd();//광고재생 종료
                                break;
                        case "failedToShow":
                                Debug.Log("광고 보여주기 실패");
                                EndAd();//광고재생 종료
                                break;
                        case "impression":
                                Debug.Log("광고 노출");
                                break;
                        case "show":
                                Debug.Log("광고 컨텐츠 보여졌음");
                                break;
                        //ShowAd쪽
                        case "requested":
                                Debug.Log("광고 보여주기 요청 완료");
                                AudioManager.Instance.BGM.PauseBGM(true);//BGM일시정지
                                GameManager.Instance.PauseGame();//게임 일시정지
                                break;
                        default:
                                Debug.Log(eventType);
                                break;
                }
        }

        private void EndAd()
        {
            AudioManager.Instance.BGM.PauseBGM(false);//BGM 다시 재생
            GameManager.Instance.ResumeGame();//게임재개
            GameManager.Instance.inGameController.EndAdMob();//광고 재생으로 멈춘 루틴 재개
            UIManager.Instance.popupUIController.HideAdBg();
        }

        public void DebugText<T>(T msg)
        {
                //디버깅용
                if (UIManager.Instance.popupUIController.AdmobDebugText.gameObject.activeSelf)
                {
                        UIManager.Instance.popupUIController.SetAdmobDebugText(msg.ToString());
                }
        }
}
