//#define ITCH

using System;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;


public class NetworkManager : Singleton<NetworkManager>
{
        [DllImport("__Internal")]
        private static extern void OpenTossLeaderboard();
        
        [DllImport("__Internal")]
        private static extern void CheckTossAppVersion();
        
        [DllImport("__Internal")]
        private static extern void SubmitTossScore(int score);
        
        [DllImport("__Internal")]
        private static extern void LoadInterstitialAd();
        [DllImport("__Internal")]
        private static extern void ShowInterstitialAd();
        
        private bool _adLoaded = false;
        private int _adRetryCount;

        protected override void Initialize()
        {
#if UNITY_WEBGL && !UNITY_EDITOR //Toss(WebGL) 버전일때만 로직 수행
                _adLoaded = false;
                _adRetryCount = 0;
                LoadAd();
#endif
        }
        
        /// <summary>
        /// 토스 리더보드 호출
        /// </summary>
        public void OnTossLeaderboard()
        {
#if UNITY_WEBGL && !UNITY_EDITOR  && !ITCH//WebGL 환경에서만 동작
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
        } catch (Exception e) {
            Debug.LogException(e);
        }
#else
                // 에디터/브라우저 테스트용 모킹
                Debug.Log("Submitting Score...");
#endif
        }
        
        //실패시 점수 재전송용
        private void ReSendScore()
        {
                var score = UIManager.Instance.popupUIController.resultUIController.cacheScore;//예비로 캐싱한 점수
                UIManager.Instance.ErrorReport().OnRetryProcess += ()=> SendScore(score);//재호출 액션 등록
        }
        
        // 게임 종료 시 호출
        public void ShowAd()
        {
#if UNITY_WEBGL && !UNITY_EDITOR  && !ITCH
        try {
                RaiseAd();   
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
#if UNITY_WEBGL && !UNITY_EDITOR  && !ITCH
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

        public void CheckAppVersion()
        {
#if UNITY_WEBGL && !UNITY_EDITOR  && !ITCH
       try {
                CheckTossAppVersion();  // 게임 시작 시 미리 로드
        } catch (Exception e) {
                Debug.LogException(e);
                Debug.Log("Not Supported App Version.");
        }
#else
                Debug.Log("Not Supported App Version.");
#endif
                
        }

        public void OnScoreEvent(string eventType)
        {
                Debug.Log($"스코어 이벤트 수신: {eventType}");

                //디버깅용
                if (UIManager.Instance.popupUIController.AdmobDebugText.gameObject.activeSelf)
                {
                        UIManager.Instance.popupUIController.SetAdmobDebugText(eventType);
                }
                
                //송신받은 event메시지에 따라 다른 액션
                switch (eventType)
                {
                        //토스앱 버전 체크
                        case "VersionSupported":
                                Debug.Log("지원하는 버전입니다.");
                                GameManager.Instance.isSupportedCheck = true;
                                break;
                        
                        case "VersionNotSupported":
                                Debug.Log("토스앱 버전 미지원");
                                GameManager.Instance.isSupportedCheck = true;
                                //버전 미지원은 팝업을 닫지 않는다. 앱을 닫고 업데이트를 권장한다. 팝업을 안닫음으로서 진행막기.
                                SendErrorReport("지원하지 않는 버전입니다.\n토스앱을 업데이트 해주세요.");
                                
                                break;
                        
                        //리더보드 열기
                        case "SuccessToOpenBoard":
                                Debug.Log("리더보드 열기 성공");
                                break;
                        
                        case "FailedToOpenBoard":
                                Debug.Log("리더보드 열기 실패");
                                SendErrorReport("리더보드 호출을 실패했습니다.\n네트워크 확인 후 터치하여 재시도 하세요.");
                                UIManager.Instance.ErrorReport().OnRetryProcess += OnTossLeaderboard;//재호출 액션 등록
                                break;
                        
                        //스코어 전송
                        case "SuccessToSend":
                                Debug.Log("점수 제출 성공");
                                break;
                        
                        case "FailedToSend":
                                Debug.Log("점수 제출 실패");
                                SendErrorReport("점수 전송에 실패했습니다.\n네트워크 확인 후 터치하여 재시도 하세요.");
                                ReSendScore();//점수 재전송 이벤트 등록
                                break;
                        
                        case "FailedToScoreSeq":
                                Debug.Log("점수 제출 실패");
                                SendErrorReport("점수 전송에 실패했습니다.\n네트워크 확인 후 터치하여 재시도 하세요.");
                                ReSendScore();//점수 재전송 이벤트 등록
                                break;
                        
                        //이벤트에 해당하지 않음 -> 오류 메시지 취득
                        default:
                                Debug.Log(eventType);
                                //기존 알림 메시지에 오류메시지를 뒤에 표시한다.
                                SendErrorReport($"ErrCode: {eventType}\n터치하면 팝업을 닫습니다.");
                                UIManager.Instance.ErrorReport().OnRetryProcess += ()=> UIManager.Instance.ErrorReport().ClosePopup();
                                break;
                }
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
                                _adLoaded = true; //로드 확인
                                Debug.Log("광고 로드 성공");
                                break;
                        
                        //ShowAd쪽
                        case "clicked":
                                Debug.Log("광고 클릭");
                                break;
                        
                        case "dismissed":
                                Debug.Log("광고 닫힘");
                                EndAd();//광고재생 종료 루틴
                                break;
                        
                        case "failedToShow":
                                Debug.Log("광고 보여주기 실패");
                                if(_adRetryCount < 3) StartCoroutine(AdReload());//광고 재로드 및 재생.
                                else {
                                        SendErrorReport("광고 재생에 실패했습니다.");//광고재생 종료 루틴 -> 나중에 불러오기 실패 팝업으로 바꾸기
                                        UIManager.Instance.ErrorReport().ClosePopup(true);//팝업 자동 닫기
                                        EndAd();//광고 루틴 종료
                                }
                                break;
                        
                        case "impression":
                                Debug.Log("광고 노출");
                                break;
                        
                        case "show":
                                Debug.Log("광고 컨텐츠 보여졌음");
                                break;
                       
                        case "requested":
                                Debug.Log("광고 보여주기 요청 완료");
                                AudioManager.Instance.BGM.PauseBGM(true);//BGM일시정지
                                GameManager.Instance.PauseGame();//게임 일시정지
                                break;
                        
                        default:
                                Debug.Log(eventType);
                                SendErrorReport($"ErrCode: {eventType}\n터치하면 팝업을 닫습니다.");
                                UIManager.Instance.ErrorReport().OnRetryProcess += ()=> UIManager.Instance.ErrorReport().ClosePopup();
                                break;
                }
        }

        //광고 재시작 루틴
        IEnumerator AdReload()
        {
                _adRetryCount++;
                
                LoadAd();
                while (!_adLoaded)
                {
                        yield return null;
                }
                RaiseAd();
        }
        
        //광고 재생 루틴
        private void RaiseAd()
        {
                ShowInterstitialAd();
                UIManager.Instance.popupUIController.ShowAdBg();
                _adLoaded = false;
        }

        //광고 종료 루틴
        private void EndAd()
        {
            _adRetryCount = 0;
            AudioManager.Instance.BGM.PauseBGM(false);//BGM 다시 재생
            GameManager.Instance.ResumeGame();//게임재개
            GameManager.Instance.inGameController.EndAdMob();//광고 재생으로 멈춘 루틴 재개
            UIManager.Instance.popupUIController.HideAdBg();
        }

        public void SendErrorReport(string msg)
        {
                UIManager.Instance.ErrorReport().ShowPopup();
                UIManager.Instance.ErrorReport().SetErrorText(msg);
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
