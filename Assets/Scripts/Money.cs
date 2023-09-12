using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour , IInteractable
{
    public int value;

    public void Interact()
    {
        Player.instance.IncrementMoney(value);
        Destroy(gameObject);
    }
}
