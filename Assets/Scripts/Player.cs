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

    [Header("Weapon Selectors")]
    public List<GameObject> weaponSelectors;
    public List<int> weaponChoosingInitYearsLimit;

    [Header("Upgrade Index")]
        [Tooltip("Save & Load Value")]
    // we will save and load thorugh this header and set the values after

    public int fireRateValueIndex;
    public int fireRangeValueIndex, initYearValueIndex, incomeValueIndex;
    public int money;
    public int currentLevelIndex;
    public float playerDamage;

    // public GameObject startingWeapon;

    [HideInInspector]
    public int weaponIndex;

    [Header("UpgradePhase")]
    [SerializeField] GameObject WStransfromPrefab;
    [SerializeField] List<Transform> weaponSelectorsTransformPositive;
    [SerializeField] List<Transform> weaponSelectorsTransformNegative;
    [SerializeField] List<GameObject> WSPositiveList,WSNegativeList;
    public bool positiveTurn, negativeTurn;

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
        tag = "Player";
        LoadPlayerData();
        SetUpgradedValues();

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
    public void PlayerDeath()
    {

        for (int i = 0; i < weaponSelectors.Count; i++)
        {
            weaponSelectors[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            weaponSelectors[i].GetComponent<Rigidbody>().useGravity = true;
            weaponSelectors[i].GetComponent<BoxCollider>().enabled = true;
        }

        currentLevelIndex++;

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

         /*   weaponSelectors[0].
                GetComponent<WeaponSelector>().SetStartingValues();   
            weaponSelectors[0].GetComponent<WeaponSelector>(). WeaponSelecting(); */
    }

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
