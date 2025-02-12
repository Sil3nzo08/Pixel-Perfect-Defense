using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Banner : MonoBehaviour
{
    public float movementSpeed;
    public Vector3 direction;
    public float raiseHeight;
    public float currentHeight;
    public bool hasRaisedBanner;
    public float destroyTimeAfterRaise;
    
    void Update()
    {
        RaiseBanner();
    }

    private IEnumerator DestroyBanner(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }
    
    private void RaiseBanner()
    {   
        if (!hasRaisedBanner)
        {
            transform.Translate(movementSpeed * Time.deltaTime * direction);
            currentHeight += movementSpeed * Time.deltaTime;

            if (currentHeight >= raiseHeight)
            {
                StartCoroutine(DestroyBanner(destroyTimeAfterRaise));
                hasRaisedBanner = true;
            }
        }
    }
}
