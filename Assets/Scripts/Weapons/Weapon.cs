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
    public bool isLeftPunchTurn, isRightPunchTurn;
    public bool isPunchReturned = true;

    [Header("Visual")]
    public GameObject leftHandGlove,rightHandGlove;

    private void Start() 
    {
        leftPunch.GetComponent<Punch>().SetRelatedWeapon(gameObject);
        rightPunch.GetComponent<Punch>().SetRelatedWeapon(gameObject);
        isLeftPunchTurn = true;
    }

    private void Update() 
    {
        if(!GameManager.instance.gameHasStarted) return;
        if(GameManager.instance.gameHasEnded) return; 
        
        if(!isPunchReturned) return;
    
        if(Player.instance.isKnockbacked)
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
        if(isLeftPunchTurn)
        {
            leftPunch.GetComponent<Punch>().Strike();
            leftPunch.GetComponent<Punch>().firedPoint = transform;

            // animation
            Player.instance.PlayPunchingAnim(Player.instance.leftHandController,1,0);
         

            Player.instance.currentWeapon.GetComponent<Weapon>().isLeftPunchTurn = false;
            Player.instance.currentWeapon.GetComponent<Weapon>().isPunchReturned = false;
            
        } 
        else
        {
            rightPunch.GetComponent<Punch>().Strike();
            rightPunch.GetComponent<Punch>().firedPoint = transform;


            Player.instance.PlayPunchingAnim(Player.instance.righthandController,1,rightPunch.GetComponent<Punch>().moveDur);
            


            Player.instance.currentWeapon.GetComponent<Weapon>().isLeftPunchTurn = true;
            Player.instance.currentWeapon.GetComponent<Weapon>().isPunchReturned = false;
        }       
    }
    
    public float GetWeaponsFireRange()
    {
        return Player.instance.GetInGameFireRange() + fireRange;
    }
    public float GetWeaponsFireRate()
    {
        return Player.instance.GetInGameFireRate() + fireRate;
    }
    private void UpdateFireRate()
    {
        currentFireRate = Player.instance.GetInGameFireRate() + fireRate;
    }

}
