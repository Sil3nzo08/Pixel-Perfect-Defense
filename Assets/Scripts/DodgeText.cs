using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeText : MonoBehaviour
{
    public float raisingSpeed;
    public float raiseHeight;

    private void Update()
    {
        
    }

    private IEnumerator RaiseText()
    {
        while (true)
        {
            transform.position += new Vector3(0, raisingSpeed * Time.deltaTime, 0);
        }   
    }
}
