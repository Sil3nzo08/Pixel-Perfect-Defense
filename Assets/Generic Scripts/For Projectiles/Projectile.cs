using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[System.Serializable]
public class Projectile : MonoBehaviour
{
    public string projectileName;
    public int projectileID;
    public int projectileDamage;
    public bool alreadyHitEnemy;

    // =============== Moving Projectile ===================
    public float movementSpeed;

    protected virtual void Move()
    {
        transform.Translate(movementSpeed * Time.deltaTime * Vector3.up);

        if (IsOutOfBounds() == true)
        {
            Destroy(gameObject);
        }
    }

    private bool IsOutOfBounds()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        float[] mapBounds = GameManager.instance.mapBounds;

        return xPos < mapBounds[0] || xPos > mapBounds[1] || yPos > mapBounds[2] || yPos < mapBounds[3];
    }

    // ====================== Update ============================

    private void Update()
    {
        Move();
    }   
}
