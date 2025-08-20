using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class ObstacleClearEffect
{
    //스프라이트들
    private List<Sprite> _bugAnim = new List<Sprite>();
    private List<Sprite> _handAnim = new List<Sprite>();
    private List<Sprite> _envelopeAnim = new List<Sprite>();
    
    //로드 확인용
    private bool _loadComplete = false;

    public IEnumerator LoadData()
    {
        LoadSprites();
        while (_loadComplete == false)
        {
            yield return null;
        }
    }

    //이미지 모두 로드.
    private async void LoadSprites()
    {
        //벌레
        _bugAnim.Add(await DataManager.Instance.LoadSpriteData(Addresses.Sprites.Bug.Dead));//0
        var bugAnims = await DataManager.Instance.LoadSpritesData(Addresses.Sprites.Bug.Anim);
        _bugAnim.Add(bugAnims[0]);//1
        _bugAnim.Add(bugAnims[1]);//2
        
        //손
        var handAnims = await DataManager.Instance.LoadSpritesData(Addresses.Sprites.Hand.Anim);
        _handAnim.Add(handAnims[0]);//0
        _handAnim.Add(handAnims[1]);//1
        
        //서류봉투
        var envelopeAnims = await DataManager.Instance.LoadSpritesData(Addresses.Sprites.Envelope.Anim);
        _envelopeAnim.Add(envelopeAnims[0]);//0
        _envelopeAnim.Add(envelopeAnims[1]);//1
        
        _loadComplete = true;
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
        yield return null;
    }
    
    //장애물 터치 시 연출
    public IEnumerator HitAnim(ObstacleController obstacle, int id)
    {
        Action obstacleDefuseAction = null;
        var obsRect = obstacle.transform as RectTransform;
        var totalDuration = 0f;
        
        
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
        yield return null;
    }
    
    //장애물 처리 이펙트
    public IEnumerator DefuseEffect(ObstacleController obstacle, int id)
    {
        Action obstacleDefuseAction = null;
        var seq = DOTween.Sequence();
        var obsRect = obstacle.transform as RectTransform;
        var totalDuration = 0f;
        
        //방향 기본 설정
        var rad = Random.Range(0, 360) * Mathf.Deg2Rad;
        var dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                 
        //회전 값 기본 설정
        var anglePerSecond = 360f * 5f;
        
        Debug.Log(id);
        switch (id)
        {
            //벌레
            case 0:
                 //TODO: 눈이 X로 바뀌며 빙글빙글 돌며 바깥으로 날아간다.
                 obstacle.obstacleImage.sprite = _bugAnim[0];//기절 스프라이트
                 totalDuration = 1f;//지속시간 설정
                 
                 //개별 설정
                 var bugThrowSpeed = 100f;
                 
                 //행동할 액션 등록
                 obstacleDefuseAction = () =>
                 {
                     //--SFX 추가 삽입구간--//
                     
                     //---------------------//
                     obstacle.transform.DORotate(new Vector3(0,0,anglePerSecond), 1f, RotateMode.FastBeyond360)
                         .SetRelative(true)
                         .SetEase(Ease.Linear)
                         .SetLoops((int)totalDuration, LoopType.Restart);;
                     obsRect.DOMove(dir*bugThrowSpeed, totalDuration).SetEase(Ease.Linear);
                 };
                break;
            
            //포스트잇
            case 1:
                //TODO: 바깥쪽으로 날아간다.
                totalDuration = 1f;//지속시간 설정
                
                //개별 설정
                var postitThrowSpeed = 50f;
                //dir = obstacle.transform.position + dir;
                
                //행동할 액션 등록
                obstacleDefuseAction = () =>
                {
                    //--SFX 추가 삽입구간--//
                     
                    //---------------------//
                    obsRect.DOMove(dir*postitThrowSpeed, totalDuration).SetEase(Ease.Linear);
                };
                break;
            
            //손
            case 2:
                //TODO: 아야하는 스프라이트로 바뀌며 진행하며 바깥으로 밀려나고 사라진다.
                obstacle.obstacleImage.sprite = _handAnim[1];//맞은 스프라이트
                totalDuration = 2f;//지속시간 설정
                
                //개별 설정
                var subDuration = 0.5f;
                var handMoveSpeed =100f;
                dir = new Vector3(dir.x, dir.y, 0);
                
                //행동할 액션 등록
                obstacleDefuseAction = () =>
                {
                    //--SFX 추가 삽입구간--//
                     
                    //---------------------//
                    seq.Append(obsRect.DOMove(dir*handMoveSpeed, subDuration).SetEase(Ease.Linear));
                    seq.Join(obsRect.DOShakeAnchorPos(subDuration, 100f, 30, 360));
                };
                break;
            
            //파일철
            case 3:
                //TODO: 진동여러번 하다가 "파일철이 바깥으로 빠진다."
                
                break;
            
            //서류봉투
            case 4:
                //TODO: 1단 서류봉투 열림, 2단 서류 삐져나옴, 3~5단 진동 마지막 단에는 "서류봉투가 아래로빠진다."
                //여기는 완전 제거연출이니까 봉투 빠지는걸로.
                
                
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
        yield return null;
    }
}