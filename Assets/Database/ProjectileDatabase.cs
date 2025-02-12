using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDatabase : MonoBehaviour
{
    // Singleton stuff
    public static ProjectileDatabase instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Actual Content
    public GameObject[] projectiles;
}
