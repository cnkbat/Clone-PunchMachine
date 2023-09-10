using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CoinGate : DamagableObject,IInteractable,IDamagable
{
    [SerializeField] List<int> coinStartingValues;
    [SerializeField] int gateValue;
    [SerializeField] TMP_Text gateValueText;
    [SerializeField] int increasingValue;

    [Header("Movement")]
    [SerializeField] bool isMoveable;
    [SerializeField] float moveValue;
    [SerializeField] int maxMovementCounter;
    int movementCounter;
    void Start()
    {
        int randValue = Random.Range(0,coinStartingValues.Count);
        gateValue = coinStartingValues[randValue];
        UpdateGateValueText();
    }
    
    void UpdateGateValueText()
    {
        gateValueText.text = "+" + gateValue.ToString();
    }

    public void Interact()
    {
        Debug.Log("inter");
        Player.instance.IncrementMoney(gateValue);
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        gateValue += increasingValue;
        UpdateGateValueText();
        if(isMoveable)
        {
            movementCounter+= 1;
            transform.position = new Vector3(transform.position.x,transform.position.y , transform.position.z + moveValue);
            if(movementCounter >= maxMovementCounter)
            {
                isMoveable = false;
            }
        }
    }
    
}
