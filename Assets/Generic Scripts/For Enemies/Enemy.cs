using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable]

// Going prefab based
public class Enemy : MonoBehaviour
{
    // Basic attributes
    public string enemyName;
    public int enemyID;
    public float movementSpeed;
    public int health;
    public int maxHealth;
    public GameObject deathAnimation;

    // ===================== Movement =====================
    public Vector3 currentWayPointPos;
    public int currentWayPointIndex = 0;
    public float wayPointTolerance = 0.05f;
    public float distanceTravelled;
    public Vector3 direction; // 1 unit long
    public float[] mapBounds;
    public bool isAlreadyDead = false;

    // ===================== Projectile Effects =====================
    public float railgunStunResistance = 0.0f;
    public bool isStunned = false;
    public int projectilesCurrentlyStunningEnemy = 0;
    public GameObject railgunStunEffect;

    protected virtual void Move()
    {
        if (isStunned == true) 
        {
            return;     // prevent anything below from happening. A.K.A no moving
        }

        if (NearWayPoint(wayPointTolerance) == true)
        {
            MoveToNextWayPoint();
        }

        if ( OutOfBounds(mapBounds[0], mapBounds[1], mapBounds[2], mapBounds[3]) == true )
        {
            Destroy(gameObject);
        }

        transform.Translate(movementSpeed * Time.deltaTime * direction);
        distanceTravelled += movementSpeed * Time.deltaTime;
    }
    private void MoveToNextWayPoint()
    {
        currentWayPointPos = EnemyDatabase.instance.wayPoints[currentWayPointIndex].position;
        direction = Vector3.Normalize(currentWayPointPos - transform.position);

        currentWayPointIndex++;
    }

    private bool NearWayPoint(float tolerance)
    {
        return (currentWayPointPos - transform.position).magnitude <= tolerance;
    }

    private bool OutOfBounds(float leftBound, float rightBound, float topBound, float bottomBound)
    {
        Vector3 enemyPos = transform.position;
        return enemyPos.x <= leftBound || enemyPos.x >= rightBound || enemyPos.y <= bottomBound || enemyPos.y >= topBound;
    }

    // ===================== Projectiles Hitting Enemy =====================
    private void OnTriggerEnter2D(Collider2D other)
    {
        Projectile projectileScript = other.GetComponent<Projectile>();
        if (projectileScript == null) 
        {
            return;
        }
        else if (projectileScript.GetType() == typeof(EnemyProjectile))
        {
            return;
        }

        if (projectileScript.alreadyHitEnemy == false)     // Check if the projectile is already hitting an enemy
        {
            projectileScript.alreadyHitEnemy = true;
            ProjectileHit(other.gameObject, projectileScript);

            // Apply any effects that the projectile might have.
            Type projectileType = projectileScript.GetType();   // Script does exist, so get it's type
            if (projectileType == typeof(ProjectileStun)) 
            {
                // Downcast script to subclass
                ProjectileStun script = (ProjectileStun) projectileScript;

                float stunDuration = script.baseStunDuration * ((100 - railgunStunResistance) / 100.0f);
                StartCoroutine(ApplyRailgunStun(stunDuration));
            }
        }
    }

    protected virtual void ProjectileHit(GameObject projectile, Projectile projectileInfo)
    {
        Destroy(projectile);

        // Damage shield
        if (hasOverseerShield == true)
        {
            bool isShieldAlive = shieldScript.AbsorbDamage(projectileInfo.projectileDamage);

            if (!isShieldAlive)
            {
                overseerShield.SetActive(false);
                hasOverseerShield = false;

                if (GameManager.instance.focusedEnemy == gameObject)
                {
                    UIManager.instance.HideEnemyShieldBar(shieldBar);
                }
            }
        }
        // Damage enemy itself
        else
        {
            health -= projectileInfo.projectileDamage;
        }

        if (health <= 0 && isAlreadyDead == false)
        {
            GameManager.instance.AddCash(projectileInfo.projectileDamage + health);     // e.g. If damage was 3 and health was 1, then health = -2 now: 3 + (-2) = 1, appropriately
            health = 0;

            isAlreadyDead = true;
            Death();
        }
        else if (health > 0)                                                    // If Enemy alive, add damage and update UI.
        {
            GameManager.instance.AddCash(projectileInfo.projectileDamage);

            if (GameManager.instance.focusedEnemy == gameObject)
            {
                UIManager.instance.DisplayEnemyHealthBar(enemyName, health, maxHealth, healthBar);

                if (hasOverseerShield)
                {
                    OverseerShield shieldInfo = overseerShield.GetComponent<OverseerShield>();
                    UIManager.instance.DisplayShieldHealthBar("Overseer Shield", shieldInfo.health, shieldInfo.maxHealth, shieldBar);
                }
            }
        }
    }

    public void PulseHit(Tower requestingScript, GameObject projectile, Projectile projectileInfo)
    {
        Type scriptType = requestingScript.GetType();

        // Ensure only pulse emitting scripts can damage enemies this way.
        if (scriptType == typeof(PulseEmitter))
        {   
            ProjectileHit(projectile, projectileInfo); 
        }
    }

    protected virtual void Death()
    {
        GameObject death = Instantiate(deathAnimation);
        death.transform.position = transform.position;

        if (GameManager.instance.focusedEnemy == gameObject)
        {
            UIManager.instance.HideEnemyHealthBar(healthBar);
            UIManager.instance.HideEnemyShieldBar(shieldBar);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Spawner.finalWaveEnded == false && Spawner.roundEnded == true)
        {
            GameObject[] leftoverEnemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (leftoverEnemies.Length == 0)
            {
                Spawner.finalWaveEnded = true;
            }
        }
    }

    // ===================== Player Viewing Enemy =====================
    public GameObject healthBar;
    public GameObject shieldBar;
    private void OnMouseDown()
    {
        UIManager.instance.DisplayEnemyHealthBar(enemyName, health, maxHealth, healthBar);
        GameManager.instance.focusedEnemy = gameObject;

        if (hasOverseerShield)
        {
            OverseerShield shieldInfo = overseerShield.GetComponent<OverseerShield>();
            UIManager.instance.DisplayShieldHealthBar("Overseer Shield", shieldInfo.health, shieldInfo.maxHealth, shieldBar);
        } else 
        {
            UIManager.instance.HideEnemyShieldBar(shieldBar);
        }
    }

    // ===================== Applying Projectile Effects =====================
    IEnumerator ApplyRailgunStun(float stunDuration) 
    {
        // Activate stun
        projectilesCurrentlyStunningEnemy++;
        isStunned = true;
        railgunStunEffect.SetActive(true);

        yield return new WaitForSeconds(stunDuration);

        // Deactivate stun if there are no more projectiles stunning the enemy
        projectilesCurrentlyStunningEnemy--;
        if (projectilesCurrentlyStunningEnemy == 0) 
        {
            isStunned = false;
            railgunStunEffect.SetActive(false);
        }
    }

    // ===================== Receiving Buffs from Allies =====================
    public GameObject overseerShield;
    public OverseerShield shieldScript;
    public bool hasOverseerShield = false;
    private IEnumerator CheckEnemyBuffs()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var enemy in enemies)
            {
                // Don't get itself.
                if (enemy == gameObject)
                {
                    continue;
                }

                Enemy enemyScript = enemy.GetComponent<Enemy>();

                if (enemyScript.GetType() == typeof(Overseer))
                {
                    Overseer script = (Overseer) enemyScript;

                    script.onShieldActivation -= ActivateOverseerShield;    // Remove any duplicates of this method.
                    script.onShieldActivation += ActivateOverseerShield;
                }
            }

            yield return new WaitForSeconds(4.9f);
        }
    }

    private void ActivateOverseerShield()
    {
        if (hasOverseerShield == false && overseerShield != null)
        {
            overseerShield.SetActive(true);
            hasOverseerShield = true;

            if (GameManager.instance.focusedEnemy == gameObject)
            {
                OverseerShield shieldInfo = overseerShield.GetComponent<OverseerShield>();
                UIManager.instance.DisplayShieldHealthBar("Overseer Shield", shieldInfo.health, shieldInfo.maxHealth, shieldBar);
            }
        }
    }

    // =============================== Updates =============================
    protected virtual void Start()
    {
        MoveToNextWayPoint();
        StartCoroutine(CheckEnemyBuffs());
    }

    protected virtual void Update()
    {
        Move();

        if (GameManager.instance.focusedEnemy != gameObject)
        {
            healthBar.SetActive(false);
            shieldBar.SetActive(false);
        }
    }
}
