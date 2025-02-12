using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton stuff
    public static GameManager instance;

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
    public float[] mapBounds; // Left, Right, Top, Bottom
    public Sprite[] rangeSprites;   // Good placement, Bad placement
    public int playerCash = 1000;

    // ===================================== Placing Towers ================================
    public GameObject hoveredTower;

    public void PlacingNewTower(GameObject tower)
    {
        int towerCost = tower.GetComponent<HoverTower>().actualTowerCost;
        if (hoveredTower == null && playerCash >= towerCost)
        {
            hoveredTower = Instantiate(tower);
        }
    }

    // Use negative money to subtract money!
    public void AddCash(int amount) 
    {
        playerCash += amount;
        UIManager.instance.UpdateCash();
    }

    // ===================================== Focusing On Towers ================================
    public GameObject focusedTower;
    public bool towerClickedOn;
    public bool mouseOnUpgradeUI;

    private void CheckMouseClick()
    {
        if (towerClickedOn == false)
        {
            if (mouseOnUpgradeUI == false)
            {
                UIManager.instance.RemoveUpgradeUI();
                focusedTower = null;
            }
        }
        else
        {
            towerClickedOn = false;
        }
    }

    // ===================================== Upgrading and Selling ================================
    public void UpgradeTower()
    {
        Vector3 currentTowerPos = focusedTower.transform.position;
        Tower focusedTowerScript = focusedTower.GetComponent<Tower>();
        (string upgradeName, int upgradeCost, Sprite upgradeImage) = focusedTowerScript.GetUpgradeInfo();

        if (upgradeName != "Fully Upgraded" && playerCash >= upgradeCost) // Not empty tower
        {
            Destroy(focusedTower);

            GameObject newTower = Instantiate(TowerDatabase.instance.towers[focusedTowerScript.towerIDUpgrade]);
            newTower.transform.position = currentTowerPos;
            newTower.GetComponent<Tower>().FocusOnTower();

            AddCash(-upgradeCost);
        }
    }

    public void SellTower()
    {
        Tower focusedTowerScript = focusedTower.GetComponent<Tower>();
        AddCash(focusedTowerScript.cashFromSelling);

        Destroy(focusedTower);
        UIManager.instance.RemoveUpgradeUI();
    }

    // ===================================== Base Health ================================
    public int health = 500;
    public int maxHealth = 500;
    public bool isDead;

    public void TakeDamage(GameObject enemy)
    {
        int enemyHealth = enemy.GetComponent<Enemy>().health;
        health -= enemyHealth;

        if (health <= 0)
        {
            health = 0;
            isDead = true;
            
            UIManager.instance.DisplayDeath();
        }

        UIManager.instance.UpdateHealthBar();
        Destroy(enemy);
    }

    // ===================== Player Viewing Enemy =====================
    public GameObject focusedEnemy;

    // ===================================== Pausing Game ================================
    public void PauseGame(bool decision)
    {
        if (decision == true)
        {
            Time.timeScale = 0;
        } 
        else if (decision == false)
        {
            Time.timeScale = 1;
        }
    }

    // =================================== Update ==================================
    private void Start() 
    {
        UIManager.instance.UpdateCash();
        UIManager.instance.UpdateHealthBar();
    }

    private void Update()
    {
       if (Input.GetMouseButtonDown(0))
       {
            CheckMouseClick();
       }

       if (Input.GetKeyDown(KeyCode.Q))
       {
            Destroy(hoveredTower);
       }
    }
    
}
/*
        1. Basic
        2. Fast
        3. Armoured
        4. Boss1
        5. Anchor
        6. Sprinter
        7. Randomizer
        8. Ninja
        9. Enforcer

Tower Ideas:
1. Turret ($200): Tier 0 (Moderate firerate + laser dealing 1 damage)
2. Laser Sight ($150): Tier 1 (Increased range from 5 --> 8)
3. Energized Shots ($300): Tier 2 (Better laser, dealing 3 damage)
4. Double shot ($550): Tier 3 (Shoots 2 lasers at once)
5. Rapid Fire ($1000): Tier 4 (Significantly improves firerate: 1f --> 0.6f) 

1. Railgun ($500): Tier 0 (Slow firerate + shots dealing 30 damage)  SHOOTS PLASMA BALLS.
2. Damage amplifier ($800): Tier 1 (Damage increase: 30 --> 120)
3. Faster Reload ($500): Tier 2 (Faster firerate: 5.5f --> 4.8f)
4. Improved Engineering ($1000): Tier 3 (Faster firerate: 4.8f --> 4.2f + Longer range: 11f --> 16f)
5. Shock-Inducing shots ($2000): Tier 4 (New ability: Shots stun the enemies briefly: 3 seconds)

1. Plasma Radiator ($400): Tier 0 (Medium fireRate + each pulse deals 10 damage)
2. Energized Pulses ($900): Tier 1 (Pulse damage increase: 10 --> 20 + longer range: 4f --> 5f)
3. Improved Cooling System ($1300): Tier 2 (Firerate: 1f --> 0.7f)
4. Stronger Core ($1000): Tier 3 (Longer range: 5f - 6.5f + Damage increase: 20 --> 35)
5. Overdrive ($2000): Tier 4 (Fire rate: 0.7f --> 0.3f)

RANGE CALCULATIONS: 0.086 * range value

Feedback:
Zoe - Just have better art + health bar on top of enemy.
*/