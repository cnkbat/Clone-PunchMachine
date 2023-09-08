using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PunchBags : MonoBehaviour,IDamagable
{
    
    [SerializeField] Transform leftPlatfromMovePoint;

    [Header("Sliding Gate")]
    [SerializeField] GameObject parentPunchBagManager;
    PunchBagsManager parentPunchBagsManagerComponent;
    BoxCollider boxCollider;

    void Start()
    {
        parentPunchBagManager = transform.parent.gameObject;
        parentPunchBagsManagerComponent = parentPunchBagManager.GetComponent<PunchBagsManager>();
        boxCollider = GetComponent<BoxCollider>();

        leftPlatfromMovePoint.position = new Vector3(GameManager.instance.leftPlatform.transform.position.x,
            GameManager.instance.leftPlatformHeight,transform.position.z); 
    }

    public void TakeDamage(float dmg)
    {
        if(parentPunchBagsManagerComponent.hitPoints.Count > 0)
        {
            boxCollider.enabled = false;
          //  parentPunchBagsManagerComponent.relatedSlidingGate.GetComponent<SlidingGate>().CollectBag(gameObject,leftPlatfromMovePoint);

            CollectBag(gameObject,leftPlatfromMovePoint);

            parentPunchBagsManagerComponent.
                hitPoints.Remove(parentPunchBagsManagerComponent.hitPoints[parentPunchBagsManagerComponent.hitPoints.IndexOf(gameObject.transform)]);
            
            if(parentPunchBagManager.GetComponent<PunchBagsManager>().hitPoints.Count <= 0)
            {
                parentPunchBagManager.GetComponent<PunchBagsManager>().boxCollider.enabled = false;
            }
        }
    }

    public void CollectBag(GameObject collectedBag, Transform leftPlatform)
    {
        float collectionMoveDur  = GameManager.instance.bagCollectionMoveDur;
        collectedBag.transform.parent = parentPunchBagsManagerComponent.relatedSlidingGate.transform;
        collectedBag.transform.DOMove(leftPlatform.position,collectionMoveDur).
        OnComplete(FirstStopAnim);
    }

    public void FirstStopAnim()
    {
        transform.DOMove(parentPunchBagsManagerComponent.relatedSlidingGate.GetComponent<SlidingGate>().bagCollectionStops[0].position, GameManager.instance.bagCollectionMoveDur)
        .OnComplete(SecondStopAnim);
    }
    public void SecondStopAnim()
    {
        transform.DOMove(parentPunchBagsManagerComponent.relatedSlidingGate.GetComponent<SlidingGate>().bagCollectionStops[1].position, GameManager.instance.bagCollectionMoveDur)
        .OnComplete(ThirdStopAnim);
        
    }
    public void ThirdStopAnim()
    {
        transform.DOMove(parentPunchBagsManagerComponent.relatedSlidingGate.GetComponent<SlidingGate>().bagCollectionStops[2].position, GameManager.instance.bagCollectionMoveDur)
        .OnComplete(FourthStopAnim);;

    }
    public void FourthStopAnim()
    {
        transform.DOMove(parentPunchBagsManagerComponent.relatedSlidingGate.GetComponent<SlidingGate>().bagCollectionStops[3].position, GameManager.instance.bagCollectionMoveDur)
        .OnComplete(LoadGate);

    }

    public void LoadGate()
    {
        parentPunchBagsManagerComponent.relatedSlidingGate.GetComponent<SlidingGate>().bagsInLoad.Add(gameObject);
        parentPunchBagsManagerComponent.relatedSlidingGate.GetComponent<SlidingGate>().LoadGate();
    }
}
