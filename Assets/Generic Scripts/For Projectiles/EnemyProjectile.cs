using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    public GameObject chosenTarget;

    private void Start()
    {
        chosenTarget.GetComponent<Tower>().onDeath += SelfDestruct;
    }

    private void OnDestroy() 
    {
        chosenTarget.GetComponent<Tower>().onDeath -= SelfDestruct;
    }

    private void SelfDestruct()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
