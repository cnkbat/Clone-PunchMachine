using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PunchBags : MonoBehaviour,IDamagable
{
    public List<GameObject> bags;
    BoxCollider boxCollider;
    [SerializeField] Transform leftPlatfromMovePoint;

    [Header("Sliding Gate")]
    [SerializeField] GameObject relatedSlidingGate;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        leftPlatfromMovePoint.position = new Vector3(GameManager.instance.leftPlatform.transform.position.x,
            GameManager.instance.leftPlatformHeight,transform.position.z); 
    }

    public void TakeDamage(float dmg)
    {
        if(bags.Count > 0)
        {
            int randHitValue = Random.Range(0,bags.Count);
            relatedSlidingGate.GetComponent<SlidingGate>().CollectBag(bags[randHitValue],leftPlatfromMovePoint);
            bags.Remove(bags[randHitValue]);
            if(bags.Count <= 0)
            {
                boxCollider.enabled = false;
            }
        }
    }
}
