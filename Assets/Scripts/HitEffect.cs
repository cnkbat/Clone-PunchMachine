using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitEffect : MonoBehaviour
{
    private void Start() 
    {
        transform.DOScale(GameManager.instance.hitEffectScale,GameManager.instance.hitEffectLifeTime)
        .OnComplete(() => Destroy(gameObject));
    }
}
