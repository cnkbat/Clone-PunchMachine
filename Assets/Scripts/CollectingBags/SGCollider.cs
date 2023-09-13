using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGCollider : MonoBehaviour , IInteractable
{
    [SerializeField] int givingValue;

    public void Interact()
    {
        Player.instance.IncrementInGameInitYear(givingValue);
        transform.parent.GetComponent<SlidingGate>().LockAllGates();
    }
    
}
