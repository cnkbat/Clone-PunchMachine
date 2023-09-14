using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBagsManager : DamagableObject,IInteractable
{
    public GameObject relatedSlidingGate;
    public BoxCollider boxCollider;
    
    public List<GameObject> bags;
    public void Interact()
    {
        for (int i = 0; i < bags.Count; i++)
        {
            bags[i].GetComponent<BoxCollider>().enabled = false;
        }   
    }
}
