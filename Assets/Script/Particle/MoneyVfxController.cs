using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MoneyVfxController : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private ParticleSystem particle;
    private ParticleSystemRenderer _psRenderer;
    private Vector3 originPos;
    
    private void Awake()
    {
        originPos = parent.position;
        _psRenderer = particle.GetComponent<ParticleSystemRenderer>();
        _psRenderer.enabled = false;
        GameManager.warmUpParticleLoad = WarmupParticle();
    }

    //파티클을 초기에 한번 켰다 꺼서 메모리 선할당

    private IEnumerator WarmupParticle()
    {
        var initPos = originPos + new Vector3(4000, 0, 0);
        parent.position = initPos;
        parent.gameObject.SetActive(true);
        particle.Play();
        //5프레임
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _psRenderer.enabled = true;
        parent.position = originPos;
        parent.gameObject.SetActive(false);
        GameManager.Instance.particleLoadOn = true;
    }
}
