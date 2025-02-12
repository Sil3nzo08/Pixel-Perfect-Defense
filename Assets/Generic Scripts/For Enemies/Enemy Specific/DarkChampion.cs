using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkChampion : Enemy
{
    // =========================== Activate Special Abilities ================================
    public GameObject dodgeText;
    public float dodgeChance;       // From 0.00 to 100.00

    public int dodged;
    public int notDodged;

    protected override void ProjectileHit(GameObject projectile, Projectile projectileInfo)
    {
        // Check if the champion has dodged the attack.
        if (hasDodgedAttack())
        {
            Destroy(projectile);
            GameObject text = Instantiate(dodgeText);
            text.transform.position = transform.position;

            dodged++;
            return;
        }

        notDodged++;
        base.ProjectileHit(projectile, projectileInfo);
    }

    private bool hasDodgedAttack()
    {
        // For example, if we have 25 chance, then numbers from 0 - 25 will allow the dodge to happen while 26 - 75 won't allow the dodge.
        float landedNum = Random.Range(0.00f, 100.0f);

        if (dodgeChance > landedNum)
        {   
            return true;
        }
        else 
        {
            return false;
        }
    }
}
