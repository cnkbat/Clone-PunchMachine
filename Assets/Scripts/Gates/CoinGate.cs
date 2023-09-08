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
    void Start()
    {
        int randValue = Random.Range(0,coinStartingValues.Count);
        gateValue = coinStartingValues[randValue];
        UpdateGateValueText();
    }
    
    void UpdateGateValueText()
    {
        gateValueText.text = gateValue.ToString();
    }

    public void Interact()
    {
        Player.instance.IncrementMoney(gateValue);
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        gateValue += increasingValue;
        UpdateGateValueText();
    }
}
