using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class HoverTower : MonoBehaviour
{
    // ========================== Hovering Tower ===============================
    public Camera mainCamera;
    public GameObject actualTower;  // Used for placing actual thing.
    public SpriteRenderer spriteRenderer;
    public int actualTowerCost;
    
    private IEnumerator StayOnMouse()
    {
        while (true)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Keep it in camera!
            transform.position = mousePos;

            yield return new WaitForSeconds(0.05f);
        }
    }

    private bool ValidPosition()
    {
        if (errorsInPlacement == 0 && WithinMapBoundaries() == true)
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Tower" || other.tag == "Path")
        {
            errorsInPlacement++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Tower" || other.tag == "Path")
        {
            errorsInPlacement--;
        }
    }

    private bool WithinMapBoundaries()
    {
        float[] mapBounds = GameManager.instance.mapBounds;
        Vector3 hoverTowerPos = transform.position;

        if (hoverTowerPos.x > mapBounds[0] && hoverTowerPos.x < mapBounds[1] && hoverTowerPos.y < mapBounds[2] && hoverTowerPos.y > mapBounds[3])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateRangeSprite()
    {
        if (ValidPosition() == true)
        {
            spriteRenderer.sprite = GameManager.instance.rangeSprites[0];
        }
        else
        {
            spriteRenderer.sprite = GameManager.instance.rangeSprites[1];
        }
        
    }

    // ========================== Placing Tower ===============================
    public int errorsInPlacement;

    private void PlaceTower()
    {
        GameObject placedTower = Instantiate(actualTower);
        placedTower.transform.position = transform.position;

        Destroy(gameObject);
    }

    
    // ========================== Update ===============================
    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        StartCoroutine(StayOnMouse());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ValidPosition() == true && GameManager.instance.playerCash >= actualTowerCost)
            {
                PlaceTower();

                GameManager.instance.AddCash(-actualTowerCost);
            }
        }

        UpdateRangeSprite();
    }
}
