using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Random = UnityEngine.Random;
using Unity.VisualScripting;

public class Overseer : Enemy
{
    // =============================== Activate Special Abilities =============================
    public Action onShieldActivation;
    public GameObject activatingShieldAnim;
    public float stunRange;
    public float stunDuration;

    private IEnumerator ActivateShields()
    {
        while (true)
        {
            int delay = Random.Range(8, 18);
            yield return new WaitForSeconds(delay);

            Debug.Log("Activate shield");
            if (onShieldActivation != null)
            {
                activatingShieldAnim.SetActive(true);
                onShieldActivation();
            }
        }
    }

    protected override void Death()
    {
        StunTowers();
        base.Death();
    }

    private void StunTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (var tower in towers)
        {
            float distanceDifference = (transform.position - tower.transform.position).magnitude;

            if (distanceDifference <= stunRange)
            {
                Tower script = tower.GetComponent<Tower>();
                script.ActivateStun(stunDuration);
            }
        }
    }

    // =============================== Update =============================
    protected override void Start()
    {
        base.Start();
        StartCoroutine(ActivateShields());
    }
}
