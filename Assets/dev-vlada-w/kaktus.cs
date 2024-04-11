using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kaktus : MonoBehaviour
{
    public float health = 10f; // Health of the cactus

    public void TakeDamage(float damage)
    {
        health -= damage; 
        Debug.Log("Cactus took " + damage + " damage. Remaining health: " + health);

        if (health <= 0)
        {
            Die(); 
        }
    }

    void Die()
    {
        Debug.Log("Cactus has been defeated!");
        Destroy(gameObject); // Destroy the cactus object
    }
}