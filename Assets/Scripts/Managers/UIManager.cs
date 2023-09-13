using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance {get; private set;}
    [Header("Starting Hud")]
    [SerializeField] GameObject startingHud;

    bool canHideStartingUI;
    [SerializeField] Button startButton, fireRateButton, inityearButton;


        [Tooltip("SettingButton")]

    [SerializeField] Image slidingUI;
    [SerializeField] float moveValue;

    [Header("Tap To Start")]
    [SerializeField] TMP_Text tapToStartText;
    [SerializeField] float tapToStartAnimValue, tapToStartAnimDur;
    bool biggerTurn;

    [Header("Game Hud")]
    public GameObject gameHud;

    [SerializeField] TMP_Text currentLevelText;

    [Header("GameHud Attributes")]
    [SerializeField] TMP_Text initYearNumber;
    [SerializeField] GameObject initYearImage;

    [SerializeField] TMP_Text playerMoneyText, reducerText;
    [SerializeField] float reducerMoveValue,reducerMoveDur;
    [SerializeField] Vector2 reducerTextResetPos;

    [Header("End Hud")]
    public GameObject endHud;

    [Header("Upgrades")]
    [SerializeField] TMP_Text fireRateLevelText;
    [SerializeField] TMP_Text fireRangeLevelText, incomeLevelText, initYearLevelText;
    [SerializeField] TMP_Text fireRateCostText, fireRangeCostText, incomeCostText, initYearCostText;

    [Header("Slider")]
    [SerializeField] GameObject weaponSlider;
    [SerializeField] Image fillImage;
    [SerializeField] List<GameObject> blackandWhiteImages;
    [SerializeField] List<GameObject> coloredImages;
    [SerializeField] TMP_Text currentWeaponInitYearText, nextWeaponInitYearText;

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start() 
    {
        UpdateStartingHudTexts();
        UpdateWeaponBar();

        tapToStartText.gameObject.transform.DOScaleX(tapToStartAnimValue,tapToStartAnimDur).SetLoops(-1,LoopType.Yoyo);
        

        reducerText.rectTransform.anchoredPosition = reducerTextResetPos;
        reducerText.gameObject.SetActive(false);
        
    }
    private void Update() 
    {
        if(canHideStartingUI)
        {
            HideStartingUI();
        }   
    
        initYearImage.transform.LookAt(GameManager.instance.startingCam.transform,Vector3.up);
    }

    public void UpdateWeaponBar()
    {
        for (int i = 0; i < coloredImages.Count; i++)
        {
            coloredImages[i].gameObject.SetActive(false);
        }
        
        coloredImages[Player.instance.weaponIndex].gameObject.SetActive(true);
        blackandWhiteImages[0].gameObject.SetActive(true);

        float fillValue = (float) Player.instance.GetInGameInitYear() -
           (float) Player.instance.weaponChoosingInitYearsLimit[Player.instance.weaponIndex + 1];

        fillImage.fillAmount = (fillValue + 100) / (float)100;

        UpdateWeaponBarTexts(Player.instance.weaponChoosingInitYearsLimit
            [Player.instance.weaponIndex], Player.instance.weaponChoosingInitYearsLimit[Player.instance.weaponIndex + 1]);
    }

    public void UpdateWeaponBarTexts(int currentValue,int nextValue)
    {
        currentWeaponInitYearText.text = currentValue.ToString();
        nextWeaponInitYearText.text = nextValue.ToString();
    }

    public void OnSettingsButtonPressed()
    {

        if(slidingUI.color.a > 0)
        {
            slidingUI.rectTransform.DOAnchorPos(new Vector2(slidingUI.rectTransform.anchoredPosition.x,
                slidingUI.rectTransform.anchoredPosition.y + moveValue), 0.4f);
            
            slidingUI.DOFade(0,0.3f);
        }
        else if(slidingUI.color.a <= 10)
        {
            slidingUI.rectTransform.DOAnchorPos(new Vector2(slidingUI.rectTransform.anchoredPosition.x,
                slidingUI.rectTransform.anchoredPosition.y - moveValue), 0.4f);
            slidingUI.DOFade(1,0.3f);
        }
    }
    // STARTING HUD
    public void OnPlayButtonPressed()
    {
        Debug.Log("PLAY");
        startButton.interactable = false;
        GameManager.instance.gameHasStarted = true;
        canHideStartingUI = true;
        GameManager.instance.EnableCam(GameManager.instance.mainCam);
        Player.instance.SetPlayerGameState();
    }
    private void HideStartingUI()
    {
        startButton.transform.SetParent(startingHud.transform,false);
        fireRateButton.transform.SetParent(startingHud.transform,false);
        inityearButton.transform.SetParent(startingHud.transform,false);

        startingHud.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
        if(startingHud.GetComponent<CanvasGroup>().alpha <= 0)
        {
            canHideStartingUI = false;
            startingHud.SetActive(false);
            fillImage.gameObject.SetActive(false);
        }
    }

    public void UpdateInitYearText()
    {
        initYearNumber.text = Player.instance.GetInGameInitYear().ToString();
    }

    public void UpdateMoneyText()
    {
        playerMoneyText.text = Player.instance.money.ToString();
    }
    public void UpdateStartingHudTexts()
    {
        currentLevelText.text = "Level " + (Player.instance.currentLevelIndex + 1).ToString();
        UpdateInitYearText();
        UpdateMoneyText();

        fireRateLevelText.text = "Level " + (Player.instance.fireRateValueIndex + 1).ToString();
        initYearLevelText.text = "Level " + (Player.instance.initYearValueIndex + 1).ToString();

        fireRateCostText.text = "$" + (UpgradeManager.instance.costs[Player.instance.fireRateValueIndex].ToString());
        initYearCostText.text = "$" + (UpgradeManager.instance.costs[Player.instance.initYearValueIndex].ToString());
    }
    public void UpdateEndingHudTexts()
    {
        fireRangeLevelText.text = "Level " + (Player.instance.fireRangeValueIndex + 1 ).ToString();
        incomeLevelText.text = "Level " + (Player.instance.incomeValueIndex + 1 ).ToString();

        fireRangeCostText.text = "$" + (UpgradeManager.instance.costs[Player.instance.fireRangeValueIndex].ToString());
        incomeCostText.text = "$" + (UpgradeManager.instance.costs[Player.instance.incomeValueIndex].ToString());

    }
    // Upgrades
    public void OnFireRateUpdatePressed()
    {
        Debug.Log("presssed");
        if(Player.instance.money >= UpgradeManager.instance.costs[Player.instance.fireRateValueIndex])
        {
            Player.instance.money -= UpgradeManager.instance.costs[Player.instance.fireRateValueIndex];
            Player.instance.fireRateValueIndex +=1;
            fireRateLevelText.text = "Level " + (Player.instance.fireRateValueIndex + 1).ToString();
            fireRateCostText.text = "$" + (UpgradeManager.instance.costs[Player.instance.fireRateValueIndex].ToString());
            Player.instance.SetUpgradedValues();
            UpdateMoneyText();
            Player.instance.SavePlayerData();
        }
    }
    public void OnFireRangeUpdatePressed()
    {
        print("firerange");
        if(Player.instance.money >= UpgradeManager.instance.costs[Player.instance.fireRangeValueIndex])
        {
            Player.instance.money -= UpgradeManager.instance.costs[Player.instance.fireRangeValueIndex];
            Player.instance.fireRangeValueIndex +=1;
            fireRangeLevelText.text = "Level " + (Player.instance.fireRangeValueIndex + 1).ToString();
            fireRangeCostText.text = "$" + (UpgradeManager.instance.costs[Player.instance.fireRangeValueIndex].ToString());
            Player.instance.SetUpgradedValues();
            UpdateMoneyText();
            Player.instance.SavePlayerData();

        }
    }
    public void OnInitYearUpdatePressed()
    {
        Debug.Log("İNİT YEAR");
        if(Player.instance.money >= UpgradeManager.instance.costs[Player.instance.initYearValueIndex])
        {
            Player.instance.money -= UpgradeManager.instance.costs[Player.instance.initYearValueIndex];
            Player.instance.initYearValueIndex +=1;
            initYearLevelText.text = "Level " + (Player.instance.initYearValueIndex + 1).ToString();
            initYearCostText.text = "$" + (UpgradeManager.instance.costs[Player.instance.initYearValueIndex].ToString());
            UpdateWeaponBar();
            UpdateMoneyText();
            Player.instance.SetUpgradedValues();
            UpdateInitYearText();
            Player.instance.SavePlayerData();
            
        }
    }
    public void OnIncomeUpdatePressed()
    {
        print("income");
        if(Player.instance.money >= UpgradeManager.instance.costs[Player.instance.incomeValueIndex])
        {
            Player.instance.money -= UpgradeManager.instance.costs[Player.instance.incomeValueIndex];
            Player.instance.incomeValueIndex +=1;
            incomeLevelText.text = "Level " + (Player.instance.incomeValueIndex + 1).ToString();
            incomeCostText.text = "$" + (UpgradeManager.instance.costs[Player.instance.incomeValueIndex].ToString());
            UpdateMoneyText();
            Player.instance.SetUpgradedValues();
            Player.instance.SavePlayerData();
        }
    }
    public void OnContinueButtonPressed()
    {
        GameManager.instance.LoadNextScene();
    }
    public void FinishHud()
    {
        endHud.SetActive(true);
        weaponSlider.SetActive(false);
        UpdateEndingHudTexts();
    }

    public void DisplayInitYearReduce()
    {

        // spawn olark yapalım
        reducerText.gameObject.SetActive(true);
        reducerText.rectTransform.anchoredPosition = reducerTextResetPos;
        
        reducerText.rectTransform.DOAnchorPos(new Vector2(reducerText.rectTransform.anchoredPosition.x,
            reducerText.rectTransform.anchoredPosition.y - reducerMoveValue),reducerMoveDur).
                OnPlay(() => {reducerText.DOFade(0,reducerMoveDur);}).
                    OnComplete(() => 
                    {
                        reducerText.DOFade(255,reducerMoveDur);
                        reducerText.rectTransform.anchoredPosition = reducerTextResetPos;
                        reducerText.gameObject.SetActive(false);
                    });
    }
    
}