using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    // Singleton stuff
    public static UIManager instance;

    private void Awake() 
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Actual content
    public TMP_Text cashText;
    public TMP_Text waveText;
    public Image healthBar;
    public Sprite[] healthBarModes;
    public TMP_Text healthText;
    public Image leftUpgradeScreen;
    public Image rightUpgradeScreen;
    public Sprite[] upgradeImageSprites;
    public Image enemyHealth;

    public void UpdateCash()
    {
        cashText.text = "Cash: $" + GameManager.instance.playerCash;
    }

    public void UpdateWave(bool inIntermission, int waveNum, int secondsLeftForIntermission)
    {
        if (inIntermission == true)
        {
            waveText.text = "Next Wave in " + secondsLeftForIntermission;
        }
        else
        {
            waveText.text = "Wave " + waveNum;
        }
    }

    public void UpdateHealthBar()
    {
        int currentHealth = GameManager.instance.health;
        int maxHealth = GameManager.instance.maxHealth;

        healthText.text = currentHealth.ToString();
        int percentageLeft = Mathf.RoundToInt((float)currentHealth / maxHealth * 100);
        healthBar.sprite = healthBarModes[Mathf.RoundToInt(percentageLeft / 10)];
    }

    public void DisplayUpgradeUI(Vector3 towerPos, string towerName, string upgradeName, 
                                  int upgradeCost, Sprite upgradeImage, int upgradeTier, int sellCost)
    {
        float leftBound = GameManager.instance.mapBounds[0];
        float rightBound = GameManager.instance.mapBounds[1];

        // Determine which UI to turn on
        GameObject chosenUpgradeUI = null;
        float middle = (leftBound + rightBound) / 2;

        if (towerPos.x >= middle)
        {
            chosenUpgradeUI = leftUpgradeScreen.gameObject;
            chosenUpgradeUI.SetActive(true);

            rightUpgradeScreen.gameObject.SetActive(false);
        }
        else
        {
            chosenUpgradeUI = rightUpgradeScreen.gameObject;
            chosenUpgradeUI.SetActive(true);

            leftUpgradeScreen.gameObject.SetActive(false);
        }

        // Determine "Tier" of upgrade to display: green bar
        chosenUpgradeUI.GetComponent<Image>().sprite = upgradeImageSprites[upgradeTier];

        // Put the right text and images on upgrade UI
        TMP_Text towerNameText = chosenUpgradeUI.transform.Find("TowerName").GetComponent<TMP_Text>();
        towerNameText.text = towerName;

        TMP_Text upgradeNameText = chosenUpgradeUI.transform.Find("UpgradeName").GetComponent<TMP_Text>();
        upgradeNameText.text = upgradeName;

        TMP_Text upgradeCostText = chosenUpgradeUI.transform.Find("Upgrade").transform.Find("UpgradeText").GetComponent<TMP_Text>();
        if (upgradeName == "Fully Upgraded")
        {
            upgradeCostText.text = "MAXED";
        }
        else
        {
            upgradeCostText.text = "$" + upgradeCost;
        }

        TMP_Text sellText = chosenUpgradeUI.transform.Find("Sell").transform.Find("SellText").GetComponent<TMP_Text>();
        sellText.text = "$" + sellCost;

        Image upgradeImageObj = chosenUpgradeUI.transform.Find("UpgradeImage").GetComponent<Image>();
        upgradeImageObj.sprite = upgradeImage;

        chosenUpgradeUI.SetActive(true);
    }

    public void RemoveUpgradeUI()
    {
        leftUpgradeScreen.gameObject.SetActive(false);
        rightUpgradeScreen.gameObject.SetActive(false);
    }

    public void DisplayEnemyHealthBar(string name, int currentHealth, int maxHealth, GameObject personalHealthBar)
    {
        int percentageLeft = Mathf.RoundToInt((float)currentHealth / maxHealth * 100);
        Sprite chosenHealthBarSprite = healthBarModes[Mathf.RoundToInt(percentageLeft / 10)];
        enemyHealth.sprite = chosenHealthBarSprite;
        personalHealthBar.GetComponent<SpriteRenderer>().sprite = chosenHealthBarSprite;

        enemyHealth.transform.Find("HealthText").GetComponent<TMP_Text>().text = currentHealth.ToString();
        enemyHealth.transform.Find("NameText").GetComponent<TMP_Text>().text = name;

        enemyHealth.gameObject.SetActive(true);
        personalHealthBar.SetActive(true);
    }

    public void HideEnemyHealthBar(GameObject personalHealthBar)
    {
        enemyHealth.gameObject.SetActive(false);
        personalHealthBar.SetActive(false);
    }

    // ========================== Shield UI Display ====================================
    public Sprite[] shieldBarModes;
    public Image enemyShieldBar;

    public void DisplayShieldHealthBar(string name, int currentShield, int maxShield, GameObject personalShieldBar)
    {
        int percentageLeft = Mathf.RoundToInt((float)currentShield / maxShield * 100);
        Sprite chosenShieldBarSprite = shieldBarModes[Mathf.RoundToInt(percentageLeft / 10)];
        enemyShieldBar.sprite = chosenShieldBarSprite;
        personalShieldBar.GetComponent<SpriteRenderer>().sprite = chosenShieldBarSprite;

        enemyShieldBar.transform.Find("ShieldText").GetComponent<TMP_Text>().text = currentShield.ToString();
        enemyShieldBar.transform.Find("NameText").GetComponent<TMP_Text>().text = name;

        enemyShieldBar.gameObject.SetActive(true);
        personalShieldBar.SetActive(true);
    }

    public void HideEnemyShieldBar(GameObject personalShieldBar)
    {
        enemyShieldBar.gameObject.SetActive(false);
        personalShieldBar.SetActive(false);
    }
    
    // ========================== Game Over Stuff ====================================
    public Image congratulationsScreen;
    public Image deathScreen;
    public event Action startChallenge;
    
    public void DisplayDeath()
    {
        deathScreen.gameObject.SetActive(true);
        GameManager.instance.PauseGame(true);
    }

    public void DisplayCongratulations()
    {
        if (GameManager.instance.isDead == false)
        {
            congratulationsScreen.gameObject.SetActive(true);
            GameManager.instance.PauseGame(true);
        }
    }

    public void StartEndless()
    {
        GameManager.instance.PauseGame(false);

        congratulationsScreen.gameObject.SetActive(false);
        startChallenge?.Invoke();

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        GameManager.instance.PauseGame(false);
        Spawner.roundEnded = true;

        SceneManager.LoadScene(1);
    }
}
