using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    //ending level related variables
    [Header("Phase variables")]
    public bool gameHasEnded = false;
    public bool gameHasStarted = false;
    //----------------

    [Header("Player Attributes")]
    public float playerDamage;
    public float playerHandsAnimValue, playerHandsAnimDur;

    [Header("Stickman")]
    public float stickmanFireRate;
    public float stickmanFireRange;
    public float stickmanTravelDur, stickmanTravelDist; // platformun xinde hareket edecek dist o olacak

    [Header("Sliding Gate")]
    public int numOfPunchBagInLoad;

    [Header("Ending")]
    public GameObject endWeapon;
    public Vector3 rotationSpeed = new Vector3(100, 0, 0); // Adjust the rotation speed as needed

    [Header("LevelSelector")]
    public List<GameObject> firstLevels;
    public List<GameObject> secondLevels,coinLevels;
    public int numOfPresetLevels;
    public List<int> coinLevelIndexs;
    public Transform levelSpawnTransform;

    [Header("Visual")]
    public GameObject mainCam;
    public GameObject startingCam,endingCam , currentCam;

    [Header("LeftPlatform")]
    public GameObject leftPlatform;
    public float leftPlatformHeight;

    [Header("Collecting Bags")]
    public int maxNumOfCollectingBags;
    public float bagCollectionMoveDur;

    [Header("KnockBack")]
    public int playerKnockBackValue;
    
    [Header("VFX")]
    public GameObject hitEffect;
    public float hitEffectLifeTime;
    public float hitEffectScale;
    public GameObject coinVFX;

    [Header("Balance")]
    public float fireRateDivisor;
    public float fireRangeDivisor , lowestFireRatePossible;
    public int coinLevelIndexer;

    ////
    ////   ***********<SUMMARY>*************
    //// Game manager script is alive every scene.
    /// this script manages all the scores and hat related stuff in the game
    /// and all the huds that player can and cant see.
    /// as you can see at the bottom this script also finishes the game.


    // assigning variables
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } 

        int firstCoinLevelIndex = coinLevelIndexer;
        coinLevelIndexs.Add(firstCoinLevelIndex);

        for (int i = 1; i < 1000; i++)
        {
            
            int nextCoinLevelIndex = coinLevelIndexs[i-1] + coinLevelIndexer;
            coinLevelIndexs.Add(nextCoinLevelIndex);
        }
        
    }
    private void OnEnable() 
    {
        currentCam = startingCam;    
    }
    private void Start()
    {
        LevelChooser();
        endWeapon = GameObject.FindGameObjectWithTag("EndWeapon");
    }

    public void LevelChooser()
    {
        if(coinLevelIndexs.Contains(Player.instance.currentLevelIndex) && coinLevels.Count > 0)
        {
            int levelRand = Random.Range(0,coinLevels.Count);
            Instantiate(coinLevels[levelRand],levelSpawnTransform.position,Quaternion.identity);
            return;
        }

        if(Player.instance.currentLevelIndex <= numOfPresetLevels)
        {
            Instantiate(firstLevels[Player.instance.currentLevelIndex],levelSpawnTransform.position,Quaternion.identity);
        }
        else
        {
            int levelRand = Random.Range(0,secondLevels.Count);

            Debug.Log("Level Index = " + levelRand);

            Instantiate(secondLevels[levelRand],levelSpawnTransform.position,Quaternion.identity);
        }
    }

    private void Update() 
    {
        if(endWeapon != null)
        {
            endWeapon.transform.Rotate(rotationSpeed * Time.deltaTime);
        }
        
        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("save");
            Player.instance.SavePlayerData();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            Player.instance.IncrementInGameInitYear(15);
        }
    
    } 

    public void EndLevel()
    {
        gameHasEnded = true;
        Player.instance.PlayerDeath();
        UIManager.instance.FinishHud();
    }
    
    public void EnableCam(GameObject newCam)
    {
        newCam.SetActive(true);
        currentCam = newCam;
    }

    public void FinishLinePassed()
    {
        EnableCam(endingCam);
    }
   
    // buttona basıldığında gerçekleşecek
    public void LoadNextScene()
    {
        Player.instance.SavePlayerData();
        SceneManager.LoadScene(0);
    }
    
}
