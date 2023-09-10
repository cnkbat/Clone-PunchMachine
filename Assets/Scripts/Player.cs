using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using System.IO;

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


    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } 
    }

    void Start() 
    {
        rBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        boxCollider = GetComponent<BoxCollider>();
     //   LoadPlayerData();
        SetUpgradedValues();
    //    WeaponSelector();
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
        IncrementPlayersInitYear(GameManager.instance.playerKnockBackValue);

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
        if(inGameInitYear > weaponChoosingInitYearsLimit[6] && initYear <= weaponChoosingInitYearsLimit[7] && currentWeapon != weapons[7])
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }

            weaponIndex = 7;
            currentWeapon = weapons[7];
            currentWeapon.SetActive(true);
        }
        currentWeapon.transform.parent = transform;
        UpdatePlayersDamage();
    }

    // weapon selector

    public void PlayerDeath()
    {
        // Player anime girecek
        currentLevelIndex++;
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
    
   /* public void SetWeaponsInitYearTextState(bool boolean)
    {
        for (int i = 0; i < weaponSelectors.Count; i++)
        {
            for (int a = 0; a < weaponSelectors[i].GetComponent<WeaponSelector>().weapons.Count; a++)
            {
                weaponSelectors[i].GetComponent<WeaponSelector>().weapons[a].GetComponent<Weapon>().UpdateInitYearText(boolean);
            }
        }
    } */
    
    // Getters And Setters
    public int GetInGameInitYear()
    {
        return inGameInitYear;
    }
    public float GetInGameFireRange()
    {
        return inGameFireRange;
    }
    public float GetInGateFireRate()
    {
        return inGameFireRate;
    }

    private void SetStartingValues()
    {
        inGameFireRange = fireRange;
        inGameFireRate = fireRate;
        inGameInitYear = initYear;
        
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

    // AYNI ŞEKİLDE BUNLARIN İNGAME OLANI DA LAZIM KAPI FALAN
    public void IncrementPlayersInitYear(int value)
    {
        initYear += value;
        // SetstartingValue yapcaz
    }
    public void IncrementPlayersFireRate(float value)
    {
        fireRate -= value;
        // SetstartingValue yapcaz

    }
    public void IncrementPlayersFireRange(float value)
    {
        fireRange += value;
        // SetstartingValue yapcaz
    }

    public void IncrementMoney(int value)
    {
        money += Mathf.RoundToInt(value * income);
        UIManager.instance.UpdateMoneyText();
    }
}
