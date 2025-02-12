using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalDisrupter : Enemy
{
    // ===================== Debuffing towers =====================
    public float rangePercentageLimitation;
    private IEnumerator DisruptTowers()
    {
        while (true)
        {
            GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

            foreach (var tower in towers)
            {
                Tower towerScript = tower.GetComponent<Tower>();
                towerScript.EnableRangeDisruption(rangePercentageLimitation);
            }

            yield return new WaitForSeconds(5f);
        }
    }

    private bool FindSignalDisrupters()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            // Don't get itself.
            if (enemy == gameObject)
            {
                continue;
            }

            Enemy script = enemy.GetComponent<Enemy>();
            if (script.GetType() == typeof(SignalDisrupter))
            {
                return true;
            }
        }

        return false;
    }

    protected override void Death()
    {
        base.Death();

        // Remove range disruption
        bool hasOtherSignalDisrupters = FindSignalDisrupters();
        if (!hasOtherSignalDisrupters)
        {
            GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
            foreach (var tower in towers)
            {
                Tower towerScript = tower.GetComponent<Tower>();
                towerScript.DisableRangeDisruption();
            }
        }
    }

    // ===================== Update =====================
    protected override void Start()
    {
        base.Start();
        StartCoroutine(DisruptTowers());
    }
}
