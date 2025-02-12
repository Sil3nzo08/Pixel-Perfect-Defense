using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabase : MonoBehaviour
{
    // Singleton stuff
    public static EnemyDatabase instance;

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

    // Actual content
    public GameObject[] enemies;
    public Transform[] wayPoints;
    public float[] mapBounds;
}
