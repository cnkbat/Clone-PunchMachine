using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject saw;
    [SerializeField] Vector3 rotationSpeed;

    private void Update() 
    {
        saw.transform.Rotate(rotationSpeed * Time.deltaTime);
    }
    public void Interact()
    {
        Player.instance.KnockbackPlayer();
        GetComponent<BoxCollider>().enabled = false;
    }
}
