using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlidingGate : MonoBehaviour
{
    [Header("Load")]
    public List<GameObject> bagsInLoad = new List<GameObject>();

    [SerializeField] int loadValue;
    public int firstLoadInitYear,secondLoadInitYear,thirdLoadInitYear;
    [SerializeField] BoxCollider firstBoxCol,secondBoxCol,thirdBoxCol;

    [Header("Gate")]
    [SerializeField] Material greenMat;
    [SerializeField] GameObject firstGate,secondGate,thirdGate;

    [Header("Collecting Bag")]
    public List<Transform> bagCollectionStops = new List<Transform>();
    private void Start() 
    {
        firstBoxCol.enabled = false;
        secondBoxCol.enabled = false;
        thirdBoxCol.enabled = false;  
    }

    public void LoadGate()
    {
        if(bagsInLoad.Count >= GameManager.instance.maxNumOfCollectingBags) return;
        
        // Perde kısmının nasıl olacağını buradan ayarlicaz


        if(bagsInLoad.Count >= loadValue)
        {
            UnlockGate(firstBoxCol,firstGate);
        }
        if(bagsInLoad.Count >= loadValue * 2)
        {
            UnlockGate(secondBoxCol,secondGate);
        }
        if(bagsInLoad.Count >= loadValue * 3)
        {
            UnlockGate(thirdBoxCol,thirdGate);
        }
    }

    private void UnlockGate(BoxCollider collider,GameObject gate)
    {
        collider.enabled = true;
        gate.GetComponent<MeshRenderer>().material = greenMat;
    }

    public void LockAllGates()
    {
        firstBoxCol.enabled = false;
        secondBoxCol.enabled = false;
        thirdBoxCol.enabled = false;
    }

    public void CollectBag(GameObject collectedBag, Transform leftPlatform)
    {

        float collectionMoveDur  = GameManager.instance.bagCollectionMoveDur;
        collectedBag.transform.DOMove(leftPlatform.position,collectionMoveDur * 2).
            OnComplete(() => collectedBag.transform.DOMove(bagCollectionStops[0].position,collectionMoveDur)).
            OnComplete(() => collectedBag.transform.DOMove(bagCollectionStops[1].position,collectionMoveDur)).
            OnComplete(() => collectedBag.transform.DOMove(bagCollectionStops[2].position,collectionMoveDur)).
            OnComplete(() => collectedBag.transform.DOMove(bagCollectionStops[3].position,collectionMoveDur)).
            OnComplete(() => bagsInLoad.Add(collectedBag))
            .OnComplete(LoadGate);
        Debug.Log("collectbag on move");
    }

}
