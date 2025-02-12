using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDatabase : MonoBehaviour
{
    // Singleton stuff
    public static TowerDatabase instance;

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
    public GameObject[] towers;
}
