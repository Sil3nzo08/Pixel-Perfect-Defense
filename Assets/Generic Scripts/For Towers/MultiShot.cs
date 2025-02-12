using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShot : Tower
{
    public Transform[] projectileExitPoints;

    protected override void FireProjectile()
    {
        foreach (var exitPoint in projectileExitPoints)
        {
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = exitPoint.position;
            newProjectile.transform.up = transform.up;
            newProjectile.name = projectile.name;
        }
    }
}
