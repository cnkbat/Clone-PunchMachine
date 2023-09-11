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
    public List<GameObject> levels;
    public int numOfPresetLevels;
    public Transform levelSpawnTransform;

    [Header("Visual")]
    public GameObject mainCam,startingCam,endingCam,upgradeCam;

    [Header("KnockBack")]
    public int playerKnockBackValue;
    [Header("LeftPlatform")]
    public GameObject leftPlatform;
    public float leftPlatformHeight;

    [Header("Collecting Bags")]
    public int maxNumOfCollectingBags;
    public float bagCollectionMoveDur;

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
       
    }
    private void Start()
    {
      //  LevelChooser();
        endWeapon = GameObject.FindGameObjectWithTag("EndWeapon");
    }

    public void LevelChooser()
    {
        if(Player.instance.currentLevelIndex <= numOfPresetLevels)
        {
            Instantiate(levels[Player.instance.currentLevelIndex],levelSpawnTransform.position,Quaternion.identity);
        }
        else
        {
            int levelRand = Random.Range(0,levels.Count);
            Instantiate(levels[levelRand],levelSpawnTransform.position,Quaternion.identity);
        }
    }

    private void Update() 
    {
        if(endWeapon != null)
        {
            endWeapon.transform.Rotate(rotationSpeed * Time.deltaTime);
        }
        
        if(Input.GetKey(KeyCode.A))
        {
            UIManager.instance.UpdateInitYearText();
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
