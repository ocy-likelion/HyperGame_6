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
    
    private void Awake()
    {
        _psRenderer = particle.GetComponent<ParticleSystemRenderer>();
        _psRenderer.enabled = false;
        GameManager.warmUpParticleLoad = WarmupParticle();
    }

    //파티클을 초기에 한번 켰다 꺼서 메모리 선할당

    private IEnumerator WarmupParticle()
    {
        parent.gameObject.SetActive(true);
        particle.Play();
        yield return new WaitForSeconds(2f);
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _psRenderer.enabled = true;
        parent.gameObject.SetActive(false);
        GameManager.Instance.particleLoadOn = true;
    }
}
