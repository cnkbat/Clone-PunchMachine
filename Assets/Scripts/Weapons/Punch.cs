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
    [SerializeField] Vector3 startingLocalPos;

    [Header("Testing")]
    float moveDur;
    RaycastHit hit;
    GameObject strikingObject;
    private void Start() 
    {
        startingLocalPos = transform.localPosition;
        
    }

    void Update()
    {
       // relatedBone.transform.position = transform.position;
        if(!isAttacking) return;
        if(!GameManager.instance.gameHasStarted) return;
        if(strikingObject != null) return;

        if(!(Vector3.Distance(firedPointCurrent,transform.position) > fireDist))
        {
            transform.position = new Vector3(transform.position.x , transform.position.y,transform.position.z + moveSpeed * Time.deltaTime);
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
        boxCollider.enabled = true;
        firedPointCurrent = firedPoint.position;

        moveDur = relatedWeapon.GetComponent<Weapon>().GetWeaponsFireRate() / 2;
        moveSpeed = fireDist / moveDur;

        if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hit ,fireDist))
        {
            Debug.Log(hit.collider.gameObject.name);
            
            strikingObject = hit.collider.gameObject;
            int hitRandValue = Random.Range(0,strikingObject.GetComponent<DamagableObject>().hitPoints.Count);

            transform.DOMove(strikingObject.GetComponent<DamagableObject>().hitPoints[hitRandValue].position,moveDur).
                OnUpdate(() => relatedBone.transform.position = transform.position);
        }

        isAttacking = true;
    }
    private void ReturnPunch()
    {
        // strike rotation geri düzeltcez
        isAttacking = false;
        boxCollider.enabled = false;
        strikingObject = null;
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
