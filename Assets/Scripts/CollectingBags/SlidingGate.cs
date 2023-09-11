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
    public List<GameObject> curtains = new List<GameObject>();


    private void Start() 
    {
        firstBoxCol.enabled = false;
        secondBoxCol.enabled = false;
        thirdBoxCol.enabled = false;  
    }

    public void LoadGate()
    {
        if(bagsInLoad.Count >= GameManager.instance.maxNumOfCollectingBags) return;
        
        curtains[0].SetActive(false);
        curtains.Remove(curtains[0]);

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

}
