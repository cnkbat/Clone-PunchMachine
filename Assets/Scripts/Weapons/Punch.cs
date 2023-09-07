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
    [SerializeField] GameObject relatedBone;

    [Header("Attiributes")]
    [SerializeField] float moveSpeed;
    
    private float fireDist;

    [Header("Punching")]
    [SerializeField] bool isAttacking;
    [SerializeField] Vector3 startingLocalPos, boneStartingPos;

    private void Start() 
    {
        startingLocalPos = transform.localPosition;
        boneStartingPos = relatedBone.transform.localPosition;
    }

    void Update()
    {
       // relatedBone.transform.position = transform.position;
        if(!isAttacking) return;

        if(!(Vector3.Distance(firedPointCurrent,transform.position) > fireDist))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed * Time.deltaTime);
            relatedBone.transform.position = transform.position;

        }   
        else
        {
            ReturnPunch();
        }
    }

    public void Strike()
    {
        // strike rotation vercez
        fireDist =  relatedWeapon.GetComponent<Weapon>().GetWeaponsFireRange();
        isAttacking = true;
        boxCollider.enabled = true;
        firedPointCurrent = firedPoint.position;

        float moveDur = relatedWeapon.GetComponent<Weapon>().GetWeaponsFireRate() / 2;
        moveSpeed = fireDist / moveDur;
        Debug.Log("attacking");
    }
    private void ReturnPunch()
    {
        // strike rotation geri düzeltcez
        isAttacking = false;
        boxCollider.enabled = false;
        transform.DOLocalMove(startingLocalPos, relatedWeapon.GetComponent<Weapon>().GetWeaponsFireRate() / 2).
            OnUpdate(() => relatedBone.transform.position = transform.position);
        relatedWeapon.GetComponent<Weapon>().isPunchReturned = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(Player.instance.currentPlayerDamage);
            ReturnPunch();
        }
    }
    
    public void SetRelatedWeapon(GameObject newWeapon)
    {
        relatedWeapon = newWeapon;
    }
}
