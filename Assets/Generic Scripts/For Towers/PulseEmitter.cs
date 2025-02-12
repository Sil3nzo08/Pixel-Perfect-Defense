using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class PulseEmitter : Tower
{
    public Animator pulseAnimator;
    public float animationSpeed;

    // pulseEffectHits must be less than the fireCooldown!
    public float pulseEffectHits;

    protected List<GameObject> FindTargets() 
    {
        List<GameObject> enemiesWithinRange = new List<GameObject>();
        GameObject[] enemiesOnScreen = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemiesOnScreen.Length >= 1)
        {
            foreach (var enemy in enemiesOnScreen)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                float distanceToEnemy = (transform.position - enemy.transform.position).magnitude;

                if (math.abs(distanceToEnemy) <= shootingRange)
                {
                    enemiesWithinRange.Add(enemy);
                }
            }
        }
        
        if (enemiesWithinRange.Count == 0)
        {
            return null;
        }

        return enemiesWithinRange;
    }

    protected override IEnumerator ShootTarget()
    {
        while (true)
        {
            List<GameObject> targets = FindTargets();

            if (isStunned)
            {
                targets = null;
            }

            if (targets != null)
            {
                AnimatePulse();

                yield return new WaitForSeconds(pulseEffectHits);

                foreach (var target in targets)
                {
                    // target might've died during the yield return statement above, so double check to make sure it is still alive.
                    if (target != null)
                    {
                        GameObject projectileCopy = Instantiate(projectile);
                        Projectile projectileScript = projectileCopy.GetComponent<Projectile>();
                        Enemy enemyScript = target.GetComponent<Enemy>();

                        enemyScript.PulseHit(this, projectileCopy, projectileScript);
                    }
                }

                yield return new WaitForSeconds(fireCooldown - pulseEffectHits);
            }
            else 
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    private void AnimatePulse()
    {
        pulseAnimator.enabled = true;
        pulseAnimator.Rebind();
    }

    // ============================= Update ===============================
    private void Start()
    {
        pulseAnimator.speed = animationSpeed;

        StartCoroutine(ShootTarget());
    }
}
