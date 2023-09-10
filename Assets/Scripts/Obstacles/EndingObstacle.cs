using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EndingObstacle : DamagableObject , IDamagable, IInteractable
{
    [SerializeField] float health = 10;
    [SerializeField] int moneysValue; 
    [SerializeField] TMP_Text healthText;
    bool isDestroyed = false;

    [Header("Anim")]
    [SerializeField] GameObject animBone;
    [SerializeField] Vector3 animVector;
    [SerializeField] float animDur;


    private void Start() 
    {
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        healthText.text = Mathf.RoundToInt(health).ToString();
    }

    public void Interact()
    {
        GameManager.instance.EndLevel();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        animBone.transform.DORotate(animVector,animDur,RotateMode.Fast).
            OnComplete(() => animBone.transform.DORotate(Vector3.zero,animDur,RotateMode.Fast));

        if(health <= 0)
        {
            if(isDestroyed) return;
            isDestroyed = true;

            Player.instance.IncrementMoney(moneysValue);

            Destroy(gameObject);
        }
        UpdateHealthText();
    }
}
