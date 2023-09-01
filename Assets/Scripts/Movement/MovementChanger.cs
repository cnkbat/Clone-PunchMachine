using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementChanger : MonoBehaviour, IInteractable
{
    bool inside; 
    [SerializeField] bool faster, slower;
    public void Interact()
    {
        if(!inside)
        {
            if(faster)
            {
                Player.instance.SetMovementSpeed(Player.instance.fastMovSpeed);
            }
            else if(slower)
            {
                Player.instance.SetMovementSpeed(Player.instance.slowMovSpeed);
            }
            inside = true;
        }
        else if(inside)
        {
            Player.instance.SetMovementSpeed(Player.instance.originalMoveSpeed);
            inside = false;
        }
    }
}
