using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Punch : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject relatedWeapon; 
    [SerializeField] BoxCollider boxCollider;
    public Transform firedPoint;
    private Vector3 firedPointCurrent;
    [Header("Attiributes")]
    [SerializeField] float moveSpeed;
    
    private float fireDist;

    [Header("Punching")]
    [SerializeField] bool isAttacking;
    [SerializeField] Vector3 startingLocalPos;

    private void Start() 
    {
        startingLocalPos = transform.localPosition;
    }

    void Update()
    {
        if(!isAttacking) return;

        if(!(Vector3.Distance(firedPointCurrent,transform.position) > fireDist))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed * Time.deltaTime);
        }   
        else
        {
            ReturnPunch();
        }
    }

    public void Strike()
    {
        fireDist =  relatedWeapon.GetComponent<Weapon>().GetWeaponsFireRange();
        isAttacking = true;
        boxCollider.enabled = true;
        firedPointCurrent = firedPoint.position;
    }

    private void ReturnPunch()
    {
        isAttacking = false;
        boxCollider.enabled = false;
        transform.DOLocalMove(startingLocalPos, 0.2f);
        relatedWeapon.GetComponent<Weapon>().isPunchReturned = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(Player.instance.currentPlayerDamage);
        }
    }
    
    public void SetRelatedWeapon(GameObject newWeapon)
    {
        relatedWeapon = newWeapon;
    }
}
