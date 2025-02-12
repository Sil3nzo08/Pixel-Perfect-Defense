using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Randomizer : Enemy
{
    public GameObject[] enemiesToRandomize;

    protected override void Death()
    {
        // Create new enemy
        GameObject newEnemy = Instantiate(enemiesToRandomize[Random.Range(0, enemiesToRandomize.Length)]);
        newEnemy.transform.position = transform.position;

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.currentWayPointIndex = currentWayPointIndex - 1;
        enemyScript.distanceTravelled = distanceTravelled;

        // Kill self
        GameObject death = Instantiate(deathAnimation);
        death.transform.position = transform.position;

        Destroy(gameObject);
    }
}
