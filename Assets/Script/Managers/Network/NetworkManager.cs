//#define ITCH

using System;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Serialization;


public class NetworkManager : Singleton<NetworkManager>
{
//#if UNITY_WEBGL && !UNITY_EDITOR //Toss(WebGL) 버전일때만 로직 수행
        [DllImport("__Internal")]
        private static extern void OpenTossLeaderboard();
        
        [DllImport("__Internal")]
        private static extern void CheckTossAppVersion();
        
        [DllImport("__Internal")]
        private static extern void SubmitTossScore(int score);
        

        //--광고기능 함수--//

        // [DllImport("__Internal")]
        // private static extern void LoadInterstitialAd();
        // [DllImport("__Internal")]
        // private static extern void ShowInterstitialAd();
        //광고 기능 재활성화 시 RaiseAd()의 주석도 같이 해제할것.

        /// <summary>
        ///광고기능 비활성화 중 문법 충돌 회피를 위한 더미 코드.
        ///광고 기능을 다시 활성화 시 아래의 더미코드는 삭제할 것.
        /// </summary>
        private static void LoadInterstitialAd()
        {
                //
        }
        
        private static void ShowInterstitialAd()
        {
                //
        }
        
        //----//
        
        
        
        
        [DllImport("__Internal")]
        private static extern void GetSafeAreaInsets();

        /// <summary>
        /// 광고기능 사용여부
        /// </summary>
        public bool useAd;
        
        //
        private bool _adLoaded = false;
        private bool _reloadFailed = false;
        private int _adRetryCount;
        private int _adRetryLimitCount; //광고로드 재시도 상한

        protected override void Initialize()
        {
                useAd = false; //false = 광고 비활성화.
                if (!useAd) return;

#if UNITY_WEBGL && !UNITY_EDITOR //Toss(WebGL) 버전일때만 로직 수행
                _adLoaded = false;
                _adRetryCount = 0;
                _adRetryLimitCount = 5;
                LoadAd();
#endif
        }

        /// <summary>
        /// SafeArea값을 전달 받습니다. Unity에서 직접 호출하지 않는 함수입니다.
        /// </summary>
        /// <param name="safeAreaValue">SafeAreaInsets의 Top 값을 전달 받습니다.</param>
        public void RecieveSafeAreaValue(string platformValue)
        {
                if (float.TryParse(platformValue, out float safeAreaValue))
                {
                        // 변환 성공
                        GameManager.Instance.cachedSafeAreaValue = safeAreaValue;
                        GameManager.Instance.recieveDoneSafeAreaValue = true;
                }
                else
                {
                        // 변환 실패
                        GameManager.Instance.cachedSafeAreaValue = 0;
                        GameManager.Instance.recieveDoneSafeAreaValue = true;
                }

                //디버그시만 활성화
                //UIManager.Instance.popupUIController.SetAdmobDebugText(platformValue);
        }

        public static void GetSafeAreaValue()
        {
                GetSafeAreaInsets();
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
                //광고가 사전에 Load되어 있지 않으면 ShowAd의 이벤트 메시지는 받지못한다. CannotAccessAd로 연결된다.
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
                                AudioManager.Instance.BGM.PauseBGM(false);//BGM 다시 재생
                                GameManager.Instance.ResumeGame();//게임재개
                                EndAd();//광고재생 종료 루틴
                                break;
                        
                        case "failedToShow":
                                Debug.Log("광고 보여주기 실패");
                                TryLimitAd();
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
                        
                        //예외 메시지
                        default:
                                Debug.Log(eventType);
                                //ShowAd
                                if (eventType.Contains("CannotAccessAd"))//CannotAccessAd가 포함된 이벤트를 받으면 Load된 광고가 없을때 발생했다는 뜻.
                                {
                                        TryLimitAd();
                                }
                                //LoadAd
                                else if (eventType.Contains("failedToLoad"))
                                {
                                        //TryLimitAd("광고 로드에 실패했습니다.", true);
                                        _reloadFailed = true;
                                }
                                //AnotherMsg
                                else{
                                        SendErrorReport($"ErrCode: {eventType}\n터치하면 팝업을 닫습니다.");
                                        UIManager.Instance.ErrorReport().OnRetryProcess += ()=> UIManager.Instance.ErrorReport().ClosePopup();
                                }
                                
                                break;
                }
        }

        //광고 재시작 루틴
        IEnumerator AdReload(bool adLoadOnly = false)
        {
                _adRetryCount++;
                if(!_adLoaded) LoadAd();
                
                while (!_adLoaded)//
                {
                        if (_reloadFailed) break;//로드 실패 시 대기루틴 끊기
                        yield return null;
                }
                _reloadFailed = false;
                
                if(!adLoadOnly) RaiseAd();//true면Load만 하는 상황.
        }
        
        //광고 재생 루틴
        private void RaiseAd()//ShowAd에서 호출함.
        {
                _adLoaded = false;
                UIManager.Instance.popupUIController.ShowAdBg();
                //ShowInterstitialAd();
        }

        /// <summary>
        /// 광고 재시도 처리
        /// </summary>
        /// <param name="cmsg">입력 시 메시지를 커스텀하여 출력합니다.</param>
        /// <param name="adLoadOnly">광고 로드만 재시도할 때 True로 전환해주세요</param>
        private void TryLimitAd(string cmsg = null, bool adLoadOnly = false)
        {
                //재시도 루틴
                if(_adRetryCount < _adRetryLimitCount) StartCoroutine(AdReload(adLoadOnly));//광고 재로드 및 재생.
                else//재시도 횟수 초과 시.
                {
                        //광고 재생시점에서 호출한게 아니면 팝업을 띄우지 않음.
                        if (adLoadOnly) return;
                        
                        //재시도 횟수 초과 시 팝업 표출
                        SendErrorReport(cmsg ?? "광고 재생에 실패했습니다."); //광고재생 종료 루틴
                        UIManager.Instance.ErrorReport().ClosePopup(true, EndAd);//팝업 자동 닫기
                }
        }

        //광고 종료 루틴
        private void EndAd()
        {
            _adRetryCount = 0;
            GameManager.Instance.inGameController.EndAdMob();//광고 재생으로 멈춘 루틴 재개
            UIManager.Instance.popupUIController.HideAdBg();
        }
//#endif
        
        public bool CheckAdLoaded()
        {
                return _adLoaded;
        }

        //에러 팝업 띄우기
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
