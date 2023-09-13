using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Animations.Rigging;
using Unity.Mathematics;


public class Player : MonoBehaviour
{
    public static Player instance {get; private set;}
    //Component
    CapsuleCollider capsuleCollider;
    Rigidbody rBody;
    BoxCollider boxCollider;

    [Header("Movement")]
    [SerializeField] private float forwardMoveSpeed;
    [SerializeField] private float negativeLimitValue, positiveLimitValue,maxSwerveAmountPerFrame;
    private float _lastXPos;

    [Header("Movement Changers")]
    public bool knockbacked = false;
    [SerializeField] float knockbackValue = 10f ;
    [SerializeField] float knockbackDur = 0.4f;
    public float slowMovSpeed, fastMovSpeed, originalMoveSpeed;

    [Header("Saved Attributes")]
    public int initYear;
    public float income = 1;
    public float fireRate, fireRange;

    [Tooltip("Current Attributes")]
    private int inGameInitYear;
    private float inGameFireRate,inGameFireRange;

    [Header("Weapon")]
    [SerializeField] GameObject currentWeapon;

    [Header("Weapon Selecting")] 
    [SerializeField] List<GameObject> weapons;
    public List<int> weaponChoosingInitYearsLimit;
    [HideInInspector]
    public int weaponIndex;

    [Header("Upgrade Index")]
        [Tooltip("Save & Load Value")]
    // we will save and load thorugh this header and set the values after

    public int fireRateValueIndex;
    public int fireRangeValueIndex, initYearValueIndex, incomeValueIndex;
    public int money;
    public int currentLevelIndex;
    [SerializeField] float playerDamage;
    public float currentPlayerDamage;

    [Header("Animator")]

    [SerializeField] Animator playerAnimatorController;
    public GameObject leftHandController, righthandController,armRig;
    [SerializeField] GameObject stickman;

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } 
    }

    private void OnEnable() 
    {
        LoadPlayerData();
        SetUpgradedValues();
    }
    void Start() 
    {
        rBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        boxCollider = GetComponent<BoxCollider>();
        WeaponSelector();
        Debug.Log(weaponIndex);
        UpdatePlayersDamage();
        
        originalMoveSpeed = forwardMoveSpeed;
    }

    void Update() 
    {   
        if(!GameManager.instance.gameHasStarted) return;
        if(GameManager.instance.gameHasEnded) return;
       
        if(!knockbacked)
        {
            MoveCharacter();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {   
        if(other.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact();
        }
        else if(other.CompareTag("FinishLine"))
        {
            GameManager.instance.FinishLinePassed();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("MovementSlower"))
        {
            SetMovementSpeed(originalMoveSpeed);
        }
        else if(other.CompareTag("MovementFaster"))
        {
            SetMovementSpeed(originalMoveSpeed);
        }
    }
    private void MoveCharacter()
    {
        Vector3 moveDelta = Vector3.forward;
        if (Input.GetMouseButtonDown(0))
        {
            _lastXPos = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            float moveXDelta = Mathf.Clamp(Input.mousePosition.x - _lastXPos, -maxSwerveAmountPerFrame,
                maxSwerveAmountPerFrame);
            moveDelta += new Vector3(moveXDelta, 0, 0);
            _lastXPos = Input.mousePosition.x;
        }

        moveDelta *= Time.deltaTime * forwardMoveSpeed;

        Vector3 currentPos = transform.position;
        Vector3 newPos = new Vector3(
            Mathf.Clamp(currentPos.x + moveDelta.x, -negativeLimitValue, positiveLimitValue),
            currentPos.y,
            currentPos.z + moveDelta.z);
        transform.position = newPos;
    }

    // player knockBack
    public void KnockbackPlayer()
    {
        knockbacked = true;
        //IncrementPlayersInitYear(GameManager.instance.playerKnockBackValue);

        UIManager.instance.DisplayInitYearReduce();
        
        transform.DOMove
            (new Vector3(transform.position.x,transform.position.y, transform.position.z - knockbackValue),knockbackDur).
                OnComplete(ResetKnockback);
        
    }
    void ResetKnockback()
    {
        knockbacked = false;
    }    
    private void WeaponSelector()
    {
        if(inGameInitYear <= weaponChoosingInitYearsLimit[0] && currentWeapon != weapons[0])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }
            
            currentWeapon = weapons[0];
            weaponIndex = 0;
            currentWeapon.SetActive(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[0] && initYear <= weaponChoosingInitYearsLimit[1] && currentWeapon != weapons[1]) 
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            currentWeapon = weapons[1];
            weaponIndex = 1;
            currentWeapon.SetActive(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[1] && initYear <= weaponChoosingInitYearsLimit[2] && currentWeapon != weapons[2])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            currentWeapon = weapons[2];
            weaponIndex = 2;
            currentWeapon.SetActive(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[2] && initYear <= weaponChoosingInitYearsLimit[3] && currentWeapon != weapons[3])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 3;
            currentWeapon = weapons[3];
            currentWeapon.SetActive(true);
        }

        if(inGameInitYear > weaponChoosingInitYearsLimit[3] && initYear <= weaponChoosingInitYearsLimit[4] && currentWeapon != weapons[4])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 4;
            currentWeapon = weapons[4];
            currentWeapon.SetActive(true);
        }

        if(inGameInitYear > weaponChoosingInitYearsLimit[4] && initYear <= weaponChoosingInitYearsLimit[5] && currentWeapon != weapons[5])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 5;
            currentWeapon = weapons[5];
            currentWeapon.SetActive(true);
        }
        if(inGameInitYear > weaponChoosingInitYearsLimit[5] && initYear <= weaponChoosingInitYearsLimit[6] && currentWeapon != weapons[6])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            currentWeapon = weapons[6];
            weaponIndex = 6;
            currentWeapon.SetActive(true);
        }

        currentWeapon.transform.parent = transform;
        for (int i = 0; i < weapons.Count; i++)
        {
            if(weapons[i].GetComponent<Weapon>().leftHandGlove  != null && weapons[i].GetComponent<Weapon>().rightHandGlove != null)
            {
                weapons[i].GetComponent<Weapon>().leftHandGlove.SetActive(false);
                weapons[i].GetComponent<Weapon>().rightHandGlove.SetActive(false);
            }
        }

        if(currentWeapon.GetComponent<Weapon>().leftHandGlove  != null && currentWeapon.GetComponent<Weapon>().rightHandGlove != null)
        {
            currentWeapon.GetComponent<Weapon>().leftHandGlove.SetActive(true);
            currentWeapon.GetComponent<Weapon>().rightHandGlove.SetActive(true);
        }
        
        UpdatePlayersDamage();
        UIManager.instance.UpdateWeaponBarTexts(Player.instance.weaponChoosingInitYearsLimit
            [Player.instance.weaponIndex], Player.instance.weaponChoosingInitYearsLimit[Player.instance.weaponIndex + 1]);
    }

    // weapon selector
    public void PlayerDeath()
    {
        // Player anime girecek
        currentLevelIndex++;
        transform.DOMove
            (new Vector3(transform.position.x,transform.position.y, transform.position.z - knockbackValue/ 4),knockbackDur);
        playerAnimatorController.SetLayerWeight(playerAnimatorController.GetLayerIndex("Walking"),0);
        playerAnimatorController.SetLayerWeight(playerAnimatorController.GetLayerIndex("Punches"),0);
        stickman.transform.rotation = new Quaternion(0,0,0,0);
        armRig.GetComponent<Rig>().weight = 1;
        playerAnimatorController.SetBool("isDefeated",true);

    }
    public void UpdatePlayersDamage()
    {
       currentPlayerDamage = playerDamage + currentWeapon.GetComponent<Weapon>().damage;
    }

    // SAVE LOAD
    public void SavePlayerData()
    {
        SaveSystem.SavePlayerData(this);
    }
    
    public void LoadPlayerData()
    {
        SaveSystem.LoadPlayerData();
        PlayerData data = SaveSystem.LoadPlayerData();

        if(data != null)
        {
            currentLevelIndex = data.level;
            fireRateValueIndex = data.fireRateValueIndex;
            initYearValueIndex = data.initYearValueIndex;
            incomeValueIndex = data.incomeValueIndex;
            money = data.money;
            fireRangeValueIndex = data.fireRangeValueIndex;
        }
    }
    
    public void SetPlayerGameState()
    {
        playerAnimatorController.SetLayerWeight(playerAnimatorController.GetLayerIndex("Walking"),1);
        playerAnimatorController.SetLayerWeight(playerAnimatorController.GetLayerIndex("Punches"),1);
        stickman.transform.rotation = new Quaternion(0,0,0,0);
        armRig.GetComponent<Rig>().weight = 1;
    }

    public void PlayPunchingAnim(GameObject handController,float value , float time)
    {
        handController.GetComponent<TwoBoneIKConstraint>().weight = value;
        handController.GetComponent<ChainIKConstraint>().weight = value;
    }

    // Getters And Setters
    public int GetInGameInitYear()
    {
        return inGameInitYear;
    }
    public float GetInGameFireRange()
    {
        return inGameFireRange;
    }
    public float GetInGameFireRate()
    {
        return inGameFireRate;
    }

    private void SetStartingValues()
    {
        inGameFireRange = fireRange;
        inGameFireRate = fireRate;
        inGameInitYear = initYear;
        WeaponSelector();
    }
    public void SetMovementSpeed(float newMoveSpeed)
    {
        forwardMoveSpeed = newMoveSpeed;
    }

    public void SetUpgradedValues()
    {
        initYear = UpgradeManager.instance.initYearValues[initYearValueIndex];
        fireRate = UpgradeManager.instance.fireRateValues[fireRateValueIndex];
        fireRange = UpgradeManager.instance.fireRangeValues[fireRangeValueIndex];
        income = UpgradeManager.instance.incomeValues[incomeValueIndex];
        
        SetStartingValues();
    }

    // Ingame
    public void IncrementInGameFireRange(float value)
    {
        inGameFireRange += value;
    }
    public void IncrementCurrentFireRate(float value)
    {
        float effectiveValue = value / 100;
        Debug.Log(effectiveValue);
        inGameFireRate -= effectiveValue;
    }
    public void IncrementInGameInitYear(int value)
    {

        inGameInitYear += value;
        UIManager.instance.UpdateInitYearText();
        UIManager.instance.UpdateWeaponBar();

        WeaponSelector();
    }

    public void IncrementMoney(int value)
    {
        money += Mathf.RoundToInt(value * income);
        UIManager.instance.UpdateMoneyText();
    }
}
