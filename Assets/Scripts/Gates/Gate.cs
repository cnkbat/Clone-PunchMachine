using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Gate : DamagableObject , IDamagable , IInteractable
{
    //Variables
    [Header("Gate Values")]
    [SerializeField] float positiveValue = 4.0f;
    [SerializeField] float negativeValue = -4.0f;
    [SerializeField] float gateValue;   

    bool fireRateGate, fireRangeGate;
    public bool isGateActive;
    float damage;

    [Header("Year Gate")]
    bool yearGate;
    [SerializeField] float inityearValueMin,inityearValueMax;
    [SerializeField] float initYearClampValue;

    [Header("Materials")]
    [SerializeField] GameObject insideGate;
    [SerializeField] Material redPrimaryMaterial;
    [SerializeField] Material greenPrimaryMaterial;

    [Header("Visual")] 
    [SerializeField] TMP_Text gateOperatorText;
    [SerializeField] TMP_Text gateValueText;
    [Header("Hit Effect")]
    [SerializeField] TMP_Text damageText;
    [Header("Gate Manager")]
    public BoxCollider boxCollider;
    BoxCollider[] gatesBoxcolliders;
    void Start()
    {
        isGateActive = true;
        ChooseOperation();
        UpdateGateText();
        DamageSelectionAndTextUpdate();
        
        if(transform.parent.tag == "GateManager")
        {
            if(transform.parent.GetComponentInChildren<BoxCollider>())
            {
                gatesBoxcolliders = transform.parent.GetComponentsInChildren<BoxCollider>();
            }
        }
        
    }

    private void DamageSelectionAndTextUpdate()
    {
        if(!yearGate)
        {
            int rand = Random.Range(-3,3);
            if(rand == 0) ++rand;

            damage = rand;
            damageText.text = damage.ToString();

            if(damage < 0)
            {
                damageText.color = Color.red;
            }

        }

        else if(yearGate)
        {
            int rand = Random.Range(2,4);
            damage = rand;
            damageText.text = damage.ToString();
            gateValue = Mathf.Clamp(gateValue,-initYearClampValue,initYearClampValue);
        }

    }


    private void UpdateTheColorOfGate(Material newPrimaryColor)
    {
        // childlarını doğru bulup ona göre update edicez
        insideGate.GetComponent<MeshRenderer>().materials[0].color = newPrimaryColor.color;
    }

    private void ChooseOperation()
    {
        int chooseRand = Random.Range(0,3);
        float valueRand = Random.Range(negativeValue,positiveValue);

        gateValue = Mathf.RoundToInt(valueRand);


        // textleri de ona göre yazıcaz
        if(chooseRand == 0)
        {
            fireRangeGate = true;
            gateOperatorText.text = "Fire Range";
        }
        else if(chooseRand == 1)
        {
            fireRateGate = true;
            gateOperatorText.text = "Fire Rate";
        }
        else if(chooseRand == 2)
        {
            yearGate = true;
            gateOperatorText.text = "Init Year";
            valueRand = Random.Range(inityearValueMin,inityearValueMax);
            gateValue = Mathf.RoundToInt(valueRand);
        }

        if(gateValue >= 0)
        {
            if(insideGate.GetComponent<MeshRenderer>().materials[0].color != greenPrimaryMaterial.color)
            {
                UpdateTheColorOfGate(greenPrimaryMaterial);
            }
        }
        else if (gateValue < 0)
        {
            if(insideGate.GetComponent<MeshRenderer>().materials[0].color != redPrimaryMaterial.color)
            {
                UpdateTheColorOfGate(redPrimaryMaterial);
            }
        }
    }
  
    private void UpdateGateText()
    {
        gateValueText.text = gateValue.ToString();
    }


    void IDamagable.TakeDamage(float dmg)
    {
        gateValue += damage;

        if(yearGate)
        {
            gateValue = Mathf.Clamp(gateValue,-100,50);
        }

        if(gateValue >= 0)
        {
            UpdateTheColorOfGate(greenPrimaryMaterial);
        }
        else if (gateValue < 0)
        {
            UpdateTheColorOfGate(redPrimaryMaterial);
        }
        UpdateGateText();
    }

    public void Interact()
    {
        if(transform.parent.tag == "GateManager")
        {
            foreach (var collider in gatesBoxcolliders)
            {   
                    collider.enabled = false;
            }
        }
        
        if(isGateActive)
        {
            if(fireRateGate)
            {
                Player.instance.IncrementPlayersFireRate(gateValue);
            }
            else if(fireRangeGate)
            {
                Player.instance.IncrementPlayersFireRange(gateValue);
            }
            else if(yearGate)
            {
                Player.instance.IncrementPlayersInitYear(Mathf.RoundToInt(gateValue));
            }

            gameObject.SetActive(false);
        }
    }
}
