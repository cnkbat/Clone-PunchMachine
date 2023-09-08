using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] float fireRange;
    [SerializeField] float fireRate;
    private float currentFireRate;
    public float damage;

    [Header("Punching")]
    public GameObject leftPunch ;
    public GameObject rightPunch;
    public bool leftPunchTurn, rightPunchTurn;
    public bool isPunchReturned = true;

    private void Start() 
    {
        leftPunch.GetComponent<Punch>().SetRelatedWeapon(gameObject);
        rightPunch.GetComponent<Punch>().SetRelatedWeapon(gameObject);
        leftPunchTurn = true;
    }

    private void Update() 
    {
      /*  if(!GameManager.instance.gameHasStarted) return;
        if(GameManager.instance.gameHasEnded) return; */
        
        if(!isPunchReturned) return;
    
        if(Player.instance.knockbacked)
        {
            UpdateFireRate();
            return;
        }
        
        currentFireRate -= Time.deltaTime;
        
            if(currentFireRate <= 0)
            {
                
                Punch();
                UpdateFireRate();
            }
    }
    public void Punch()
    {
        if(leftPunchTurn)
        {
            leftPunch.GetComponent<Punch>().Strike();
            leftPunch.GetComponent<Punch>().firedPoint = transform;
           
            leftPunchTurn = false;
            isPunchReturned = false;
        } 
        else
        {
            rightPunch.GetComponent<Punch>().Strike();
            rightPunch.GetComponent<Punch>().firedPoint = transform;
            leftPunchTurn = true;
            isPunchReturned = false;
        }       
    }
    
    public float GetWeaponsFireRange()
    {
        return Player.instance.GetInGameFireRange() + fireRange;
    }
    public float GetWeaponsFireRate()
    {
        return Player.instance.GetInGateFireRate() + fireRate;
    }
    private void UpdateFireRate()
    {
        currentFireRate = Player.instance.GetInGateFireRate() + fireRate;
    }

}
