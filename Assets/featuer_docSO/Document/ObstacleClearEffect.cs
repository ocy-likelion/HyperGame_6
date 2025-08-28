using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class ObstacleClearEffect
{
    //스프라이트들
    private List<Sprite> _bugAnim = new List<Sprite>();
    private List<Sprite> _handAnim = new List<Sprite>();
    private List<Sprite> _envelopeAnim = new List<Sprite>();
    
    //퇴장 속도
    [NonSerialized] public float bugThrowSpeed;
    [NonSerialized] public float postitThrowSpeed;
    [NonSerialized] public float handMoveSpeed;
    [NonSerialized] public float filerOutSpeed;
    [NonSerialized] public float envelopeOutSpeed;
    
    //로드 확인용
    private bool _loadComplete = false;

    public void Initialize()
    {
        bugThrowSpeed = 1000f;
        postitThrowSpeed = 1000f;
        handMoveSpeed = 1000f;
        filerOutSpeed = 500f;
        envelopeOutSpeed = 1000f;
    }

     //이미지 모두 로드.
     public async Task LoadSprites()
     {
         //벌레
         _bugAnim.Add(await DataManager.Instance.LoadSpriteData(Addresses.Sprites.Obstacles.Bug.Dead));//0
         var bugAnims = await DataManager.Instance.LoadSpritesData(Addresses.Sprites.Obstacles.Bug.Anim);
         _bugAnim.Add(bugAnims[0]);//1
         _bugAnim.Add(bugAnims[1]);//2
         
         //손
         var handAnims = await DataManager.Instance.LoadSpritesData(Addresses.Sprites.Obstacles.Hand.Anim);
         _handAnim.Add(handAnims[0]);//0
         _handAnim.Add(handAnims[1]);//1
         
         //서류봉투
         var envelopeAnims = await DataManager.Instance.LoadSpritesData(Addresses.Sprites.Obstacles.Envelope.Anim);
         _envelopeAnim.Add(envelopeAnims[0]);//0
         _envelopeAnim.Add(envelopeAnims[1]);//1
     }

    public void ClearSprites()
    {
        _bugAnim.Clear();
        _handAnim.Clear();
    }

    //장애물 평시 애니메이션
    public IEnumerator IdleAnim(ObstacleController obstacle, int id)
    {
        Action obstacleDefuseAction = null;
        var obsRect = obstacle.transform as RectTransform;
        var totalDuration = 0f;

        if (id != 0) yield break; //벌레만 존재함.
        
        var wingStack = 0;
        while (obstacle.GetProcessCount() > 0)
        {
            obstacle.obstacleImage.sprite = _bugAnim[1];
            if(wingStack != 0 && wingStack % 3 == 0) yield return new WaitForSeconds(3f);
            //obstacleDefuseAction();
            obstacle.obstacleImage.sprite = _bugAnim[2];
            yield return new WaitForSeconds(0.1f);
            obstacle.obstacleImage.sprite = _bugAnim[1];
            yield return new WaitForSeconds(0.1f);

            wingStack++;
        }
        
        yield return null;
    }
    
    //장애물 터치 시 연출
    public IEnumerator HitAnim(ObstacleController obstacle, int id)
    {
        Action obstacleDefuseAction = null;
        var obsRect = obstacle.transform as RectTransform;
        var totalDuration = 0f;
        var envelopeStack = obstacle.GetProcessCount();

        var shakeStrength = 100f;
        var vibratoStrength = 60;
        var randomness = 90;

        if (id is 0 or 1) yield break; //벌레, 포스트잇은 갯수증가 이므로 제외.
        if(envelopeStack == 0) yield break; //0이면 제거단계라서 중단
        
        switch (id)
        {
            //손
            case 2:
                //TODO: 진동
                totalDuration = 0.1f;
                
                //행동할 액션 등록
                obstacleDefuseAction = () =>
                {
                    //--SFX 추가 삽입구간--//
                    AudioManager.Instance.SFX.PlayObsProcessTry();
                    //---------------------//
                    obsRect.DOShakePosition(totalDuration, shakeStrength, vibratoStrength, randomness);
                };
                break;
            
            //파일철
            case 3:
                //TODO: 진동여러번
                totalDuration = 0.1f;//지속시간 설정
                
                //행동할 액션 등록
                obstacleDefuseAction = () =>
                {
                    //--SFX 추가 삽입구간--//
                    AudioManager.Instance.SFX.PlayObsProcessTry();
                    //---------------------//
                    obsRect.DOShakePosition(totalDuration, shakeStrength, vibratoStrength, randomness);
                };
                break;
            
            //서류봉투
            case 4:
                //TODO: 1단 서류봉투 열림, 2단 서류 삐져나옴, 3단 진동 마지막 단에는 "서류봉투가 아래로빠진다."
                //여기는 완전 제거연출이니까 봉투 빠지는걸로.
                totalDuration = 0.1f;//지속시간 설정
                
                //개별 설정
 
                
                //행동할 액션 등록
                obstacleDefuseAction = () =>
                {
                    //--SFX 추가 삽입구간--//
                    AudioManager.Instance.SFX.PlayObsProcessTry();
                    //---------------------//
                    if (envelopeStack == 1) obstacle.obstacleImage.sprite = _envelopeAnim[1];
                    obsRect.DOShakePosition(totalDuration, shakeStrength, vibratoStrength, randomness);
                };
                break;
        }

        //액션 실행
        obstacleDefuseAction?.Invoke();

        yield return null;
    }
    
    //장애물 처리 이펙트
    public IEnumerator DefuseEffect(ObstacleController obstacle, int id)
    {
        Action obstacleDefuseAction = null;
        Action endAction = null;
        var seq = DOTween.Sequence();
        var obsRect = obstacle.transform as RectTransform;
        var totalDuration = 0f;
        var targetPos = Vector3.zero;
        
        //방향 기본 설정
        var rad = Random.Range(0, 360) * Mathf.Deg2Rad;
        var dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                 
        //회전 값 기본 설정
        var anglePerSecond = 360f * 5f;
        
        switch (id)
        {
            //벌레
            case 0:
                 //TODO: 눈이 X로 바뀌며 빙글빙글 돌며 바깥으로 날아간다.
                 obstacle.obstacleImage.sprite = _bugAnim[0];//기절 스프라이트
                 endAction = ()=> obstacle.obstacleImage.sprite = _bugAnim[1];
                 totalDuration = 0.15f;//지속시간 설정
                 
                 //개별 설정
                 targetPos = obstacle.transform.position + new Vector3(dir.x,dir.y,0) * bugThrowSpeed;
                 
                 //행동할 액션 등록
                 obstacleDefuseAction = () =>
                 {
                     //--SFX 추가 삽입구간--//
                     AudioManager.Instance.SFX.PlayObsBugPostHit();
                     //---------------------//
                     obstacle.transform.DORotate(new Vector3(0,0,anglePerSecond), 1f, RotateMode.FastBeyond360)
                         .SetRelative(true)
                         .SetEase(Ease.Linear)
                         .SetLoops((int)totalDuration, LoopType.Restart);;
                     obsRect.DOMove(targetPos, totalDuration).SetEase(Ease.Linear);
                 };
                break;
            
            //포스트잇
            case 1:
                //TODO: 바깥쪽으로 날아간다.
                totalDuration = 0.15f;//지속시간 설정
                
                //개별 설정;
                targetPos = obstacle.transform.position + new Vector3(dir.x,dir.y,0) * postitThrowSpeed;
                
                //행동할 액션 등록
                obstacleDefuseAction = () =>
                {
                    //--SFX 추가 삽입구간--//
                    AudioManager.Instance.SFX.PlayObsBugPostHit();
                    //---------------------//
                    obsRect.DOMove(targetPos, totalDuration).SetEase(Ease.Linear);
                };
                break;
            
            //손
            case 2:
                //TODO: 아야하는 스프라이트로 바뀌며 진행하며 바깥으로 밀려나고 사라진다.
                obstacle.obstacleImage.sprite = _handAnim[1];//맞은 스프라이트
                endAction = () => obstacle.obstacleImage.sprite = _handAnim[0];
                totalDuration = 0.4f;//지속시간 설정
                
                //개별 설정
                var subDuration = 0.25f;
                targetPos = obstacle.transform.position + Vector3.right * handMoveSpeed;
                
                //행동할 액션 등록
                obstacleDefuseAction = () =>
                {
                    //--SFX 추가 삽입구간--//
                    AudioManager.Instance.SFX.PlayObsHandHit();
                    //---------------------//
                    obsRect.DOShakePosition(subDuration, 200f, 50, 360).OnComplete(() =>
                    {
                        obsRect.DOMove(targetPos, 0.1f).SetEase(Ease.Linear);
                    });

                };
                break;
            
            //파일철
            case 3:
                //TODO: 진동여러번 하다가 "파일철이 바깥으로 빠진다."
                totalDuration = 0.15f;//지속시간 설정
                
                //개별 설정
                targetPos = obstacle.transform.position + Vector3.left * filerOutSpeed;
                
                //행동할 액션 등록
                obstacleDefuseAction = () =>
                {
                    //--SFX 추가 삽입구간--//
                    AudioManager.Instance.SFX.PlayObsFileEnvelopeOut();
                    //---------------------//
                    obsRect.DOMove(targetPos, totalDuration).SetEase(Ease.Linear);
                };
                break;
            
            //서류봉투
            case 4:
                //TODO: 1단 서류봉투 열림, 2단 서류 삐져나옴, 3~5단 진동 마지막 단에는 "서류봉투가 아래로빠진다."
                //여기는 완전 제거연출이니까 봉투 빠지는걸로.
                obstacle.obstacleImage.sprite = _envelopeAnim[1];//오픈
                endAction = ()=> obstacle.obstacleImage.sprite = _envelopeAnim[0];
                totalDuration = 0.15f;//지속시간 설정
                
                //개별 설정
                targetPos = obstacle.transform.position + Vector3.down * envelopeOutSpeed;
                
                //행동할 액션 등록
                obstacleDefuseAction = () =>
                {
                    //--SFX 추가 삽입구간--//
                    AudioManager.Instance.SFX.PlayObsFileEnvelopeOut();
                    //---------------------//
                    obsRect.DOMove(targetPos, totalDuration).SetEase(Ease.Linear);
                };
                break;
        }

        var elapsedTime = 0f;
        if (obstacleDefuseAction != null)
        {
            obstacleDefuseAction();
            while (totalDuration > elapsedTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
        endAction?.Invoke();
        yield return null;
    }

    //애님이 있는 경우 기본 스프라이트로 초기화.
    public void InitAnim(ObstacleController obstacle, int id)
    {
        switch (id)
        {
            //벌레
            case 0:
                 obstacle.obstacleImage.sprite = _bugAnim[1];
                break;
            //손
            case 2:
                obstacle.obstacleImage.sprite = _handAnim[0];//맞은 스프라이트
                break;
            //서류봉투
            case 4:
                obstacle.obstacleImage.sprite = _envelopeAnim[0];//오픈
                break;
        }
    }
}