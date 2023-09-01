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

    [Header("Sliding Gate")]
    public int numOfBulletsInLoad;

    [Header("Ending")]
    public GameObject endWeapon;
    public Vector3 rotationSpeed = new Vector3(100, 0, 0); // Adjust the rotation speed as needed

    [Header("Magazine")]
    public float magazineTravelDur;

    [Header("LevelSelector")]
    public List<GameObject> levels;
    public int numOfPresetLevels;
    public Transform levelSpawnTransform;
    [Header("Visual")]
    public GameObject mainCam,startingCam,endingCam,upgradeCam;

    [Header("KnockBack")]
    public int playerKnockBackValue;

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
        UpdatePlayerDamage();
        LevelChooser();
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
    public void UpdatePlayerDamage()
    {
        playerDamage = Player.instance.playerDamage;
    }

    private void Update() 
    {
        endWeapon.transform.Rotate(rotationSpeed * Time.deltaTime);
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
        Player.instance.currentLevelIndex++;
        Player.instance.SavePlayerData();
        SceneManager.LoadScene(0);
    }
    
}
