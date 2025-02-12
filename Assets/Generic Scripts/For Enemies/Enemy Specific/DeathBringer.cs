using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should create logic so that the deathbringer can't activate special abilities while stunned.
public class DeathBringer : Enemy
{
    // =============================== Movement =============================
    protected override void Move()
    {
        if (isRaisingBanner)
        {
            return;
        }
        
        base.Move();
    }



    // =============================== Activate Special Abilities =============================
    public float activationPeriod;
    public GameObject blastProjectile;
    public GameObject requestBackup;
    public List<GameObject> backupEnemies;
    public Vector3 spawnPointPos;
    public bool isRaisingBanner = false;

    public enum Abilities 
    {
        CastFireball = 0,
        RequestBackup = 1,
    }

    public Abilities chosenAbility;

    private IEnumerator ActivateSpecialAbilities()
    {
        while (true)
        {
            if (!isStunned)
            {
                chosenAbility = (Abilities) Random.Range(0, 2);

                if (chosenAbility == Abilities.CastFireball)
                {
                    CastFireball();
                } 
                else if (chosenAbility == Abilities.RequestBackup) 
                {
                    StartCoroutine(RaiseBanner());
                    StartCoroutine(RequestBackup(Random.Range(10, 21)));
                }
            }
            
            yield return new WaitForSeconds(activationPeriod);
        }
    }

    /// <summary>
    /// DeathBringer summons a fireball from its crystal, which is headed to destroy a random tower.
    /// </summary>
    private void CastFireball()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        if (towers.Length != 0)
        {
            GameObject chosenTower = towers[Random.Range(0, towers.Length)];
            Vector3 directionToTower = chosenTower.transform.position - transform.position;

            GameObject newFireball = Instantiate(blastProjectile);
            newFireball.transform.position = transform.position;
            newFireball.transform.up = directionToTower;
            newFireball.GetComponent<EnemyProjectile>().chosenTarget = chosenTower;
        }
    }

    private IEnumerator RaiseBanner()
    {
        isRaisingBanner = true;

        GameObject newBanner = Instantiate(requestBackup);
        newBanner.transform.position = transform.position;

        yield return new WaitForSeconds(2);

        isRaisingBanner = false;
    }

    private IEnumerator RequestBackup(int armySize)
    {
        int amountSpawned = 0;
        while (amountSpawned < armySize)
        {
            GameObject chosenEnemy = backupEnemies[Random.Range(0, backupEnemies.Count)];
            GameObject newEnemy = Instantiate(chosenEnemy);
            newEnemy.transform.position = spawnPointPos;

            amountSpawned++;
            yield return new WaitForSeconds(0.4f);
        }
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ActivateSpecialAbilities());
    }

    protected override void Update()
    {
        base.Update();
    }
}
