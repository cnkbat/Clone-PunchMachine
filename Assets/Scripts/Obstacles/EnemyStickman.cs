using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Animations.Rigging;

public class EnemyStickman : DamagableObject,IInteractable,IDamagable
{
    [Header("Attiributes")]
    [SerializeField] float fireRate;
    float currentFireRate;
    private float moveDur;
    [SerializeField] float fireDist;
    private bool canAttack = true;

    [SerializeField] GameObject punchingGlove,relatedBone, enemyArmRig;
    [SerializeField] Vector3 startingLocalPos;
    RaycastHit hit;

    [Header("Visual")]
    [SerializeField] TMP_Text healthText;
    [SerializeField] float maxHealth;
    float currentHealth;
    [SerializeField] Animator animator;
    [SerializeField] float animResetDelay;

    [Header("Drop")]
    [SerializeField] GameObject coin;
    [SerializeField] int coinValue;
    [SerializeField] Transform coinSpawnTransform;

    private void Start() 
    {
        currentHealth = maxHealth;
        currentFireRate = fireRate;
        startingLocalPos = punchingGlove.transform.localPosition;
        moveDur = fireRate / 2;

        UpdateHealthText();
    }

    private void Update() 
    {
      /*  if(!canAttack) return;
        Debug.Log("update");
        currentFireRate -= Time.deltaTime;
        if(currentFireRate <= 0)
        {
            Strike();
            currentFireRate = fireRate;
        }
           */
    }

    private void Strike()
    {
        int layerMask = LayerMask.NameToLayer("Player");
        Debug.Log("strike");
       
        if(Physics.Raycast(punchingGlove.transform.position,transform.TransformDirection(Vector3.forward),out hit ,fireDist))
        {
            Debug.Log(hit.collider.name);
            if(hit.collider.gameObject != null)
            {
                Debug.Log(hit.collider.name);
                enemyArmRig.GetComponent<Rig>().weight = 1;
                punchingGlove.transform.DOMove(hit.collider.gameObject.transform.position,moveDur).
                OnUpdate(() => relatedBone.transform.position = punchingGlove.transform.position);
            }
        }
    }

    private void ReturnPunch()
    {

        punchingGlove.transform.DOLocalMove(startingLocalPos, moveDur).
            OnUpdate(() => relatedBone.transform.position = punchingGlove.transform.position).OnComplete(() => 
                {
                    enemyArmRig.GetComponent<Rig>().weight = 0;
                });
    }

    public void Interact()
    {
        ReturnPunch();
        Player.instance.KnockbackPlayer();
        gameObject.layer = LayerMask.NameToLayer("CantCollidePlayer");
        canAttack = false;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        UpdateHealthText();
        
        StartCoroutine(PlayAnim(animResetDelay));

        if(currentHealth <= 0)
        {

            GameObject spawnedCoin = Instantiate(coin,coinSpawnTransform.position,Quaternion.identity);
            spawnedCoin.GetComponent<Money>().value = coinValue;
            Destroy(gameObject);

        }

    }

    private IEnumerator PlayAnim(float delay)
    {

        animator.SetBool("isDamaged",true);

        yield return new WaitForSeconds(delay);

        animator.SetBool("isDamaged",false);

    }
    private void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString();
    }
    
}
