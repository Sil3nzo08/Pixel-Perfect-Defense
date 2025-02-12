using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

[System.Serializable]
public class Tower : MonoBehaviour
{
    public string towerName;
    public int towerID;

    // ============================= Shooting ===============================
    public GameObject projectile;
    public float shootingRange;
    public float fullShootingRange;
    public Transform projectileExitPoint;
    public float fireCooldown;

    protected virtual GameObject FindTarget()
    {
        GameObject[] enemiesOnScreen = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemiesOnScreen.Length >= 1)
        {
            float furthestDistance = 0f;
            GameObject furthestEnemy = null;

            foreach (var enemy in enemiesOnScreen)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                float distanceToEnemy = (transform.position - enemy.transform.position).magnitude;

                if (enemyScript.distanceTravelled > furthestDistance && distanceToEnemy <= shootingRange)
                {
                    furthestEnemy = enemy;
                    furthestDistance = enemyScript.distanceTravelled;
                }
            }

            return furthestEnemy;
        }
        else
        {
            return null;
        }
    }

    protected virtual IEnumerator ShootTarget()
    {
        while (true)
        {
            GameObject target = FindTarget();

            if (isStunned)
            {
                target = null;
            }

            if (target != null)
            {
                // Aim at enemy
                Vector3 directionToEnemy = target.transform.position - transform.position;
                transform.up = directionToEnemy;

                // Fire projectile
                FireProjectile();

                yield return new WaitForSeconds(fireCooldown);
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    protected virtual void FireProjectile()
    {
        GameObject newProjectile = Instantiate(projectile);
        newProjectile.transform.position = projectileExitPoint.position;
        newProjectile.transform.up = transform.up;
        newProjectile.name = projectile.name;
    }

    // ============================= Focusing On Tower + Upgrades ===============================
    public GameObject range;                    // Formula for calculating scale of this: shootingRange * 0.086
    public int towerIDUpgrade;
    public int upgradeTier;
    public string itsUpgradeName;               // This tower's upgrade name, used for the tower below it.
    public Sprite towerImage;
    public int cost;
    public int cashFromSelling;

    private void OnMouseDown()
    {
        GameManager.instance.towerClickedOn = true; 
        FocusOnTower();
    }

    // Gets the upgradeName, upgradeCost, upgradeImage
    public (string upgradeName, int upgradeCost, Sprite upgradeImage) GetUpgradeInfo()
    {
        Tower towerInfo = TowerDatabase.instance.towers[towerIDUpgrade].GetComponent<Tower>();
        return (towerInfo.itsUpgradeName, towerInfo.cost, towerInfo.towerImage);
    }

    public void FocusOnTower()
    {
        GameManager.instance.focusedTower = gameObject;

        range.SetActive(true);
        (string upgradeName, int upgradeCost, Sprite upgradeImage) = GetUpgradeInfo();

        UIManager.instance.DisplayUpgradeUI(transform.position, towerName, upgradeName, upgradeCost, upgradeImage, upgradeTier, cashFromSelling);
    }

    // ============================= Getting hit by enemy projectiles ===============================
    public GameObject deathAnimation;
    public Action onDeath;
    public GameObject overseerStunAnimation;
    public bool isStunned = false;
    public int stunsActive;
    public bool isRangeDisrupted = false;
    public GameObject signalDisrupterEffectAnimation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyProjectile projectileScript = other.gameObject.GetComponent<EnemyProjectile>();
        if (projectileScript == null)               // Checks to see if the collision involves an enemy projectile!
        {
            return;
        }

        if (projectileScript.chosenTarget == this.gameObject)  // If this gameObject is the chosen target, explode.
        {
            onDeath?.Invoke();
            ExplodeTower();
        }
    }

    /// <summary>
    /// Destroy the tower, and display the appropriate death animation
    /// </summary>
    private void ExplodeTower()
    {
        GameObject death = Instantiate(deathAnimation);
        death.transform.position = transform.position;

        Destroy(gameObject);
    }

    public void ActivateStun(float stunTime)
    {
        StartCoroutine(StunTower(stunTime));
    }
    
    private IEnumerator StunTower(float stunTime)
    {
        overseerStunAnimation.SetActive(true);
        isStunned = true;
        stunsActive++;

        yield return new WaitForSeconds(stunTime);

        stunsActive--;
        if (stunsActive == 0)
        {
            isStunned = false;
            overseerStunAnimation.SetActive(false);
        }
    }

    public void EnableRangeDisruption(float rangeLimitation)
    {
        if (!isRangeDisrupted)
        {
            isRangeDisrupted = true;
            DisruptRange(true, rangeLimitation);
        }
    }

    public void DisableRangeDisruption()
    {
        if (isRangeDisrupted)
        {
            isRangeDisrupted = false;
            DisruptRange(false);
        }
    }

    private void DisruptRange(bool disrupt, float rangeLimitation = 0.0f)
    {
        if (disrupt)
        {
            shootingRange = fullShootingRange * (float) ((100 - rangeLimitation) / 100);
            range.transform.localScale = new Vector3((float) (shootingRange * 0.086), (float) (shootingRange * 0.086), 1);

            signalDisrupterEffectAnimation.SetActive(true);
        }
        else 
        {
            shootingRange = fullShootingRange;
            range.transform.localScale = new Vector3((float) (shootingRange * 0.086), (float) (shootingRange * 0.086), 1);

            signalDisrupterEffectAnimation.SetActive(false);
        }
    }
    
    // ============================= Update ===============================
    private void Start()
    {
        StartCoroutine(ShootTarget());
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.instance.focusedTower != gameObject)
            {
                range.SetActive(false);
            }
        }
    }
}
