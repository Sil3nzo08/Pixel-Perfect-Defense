using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverseerShield : MonoBehaviour
{
    public int health;
    public int maxHealth;

    /// <summary>
    /// A method used to have this shield take the damage instead of the enemy itself.
    /// </summary>
    /// <param name="damage"> How much damage hits the shield </param>
    /// <returns> Whether or not the damage killed the shield </returns>
    public bool AbsorbDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            return false;
        }
        else
        {
            return true;
        }
    }

    public void OnEnable()
    {
        health = maxHealth;
    }
}
