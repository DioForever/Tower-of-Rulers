using UnityEngine;

public class FireballScript : MonoBehaviour
{
    // Reference to the Spell class to get properties
    private FireSpell fireball;

    private void Start()
    {
        // Retrieve the Spell component attached to this GameObject
        fireball = GetComponent<FireSpell>();

        // Check if the Spell component is present
        if (fireball == null)
        {
            Debug.LogError("Fireball prefab is missing the FireSpell component!");
        }

        // Set the initial velocity based on the spellSpeed property
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.right * fireball.spellSpeed;
        }
        else
        {
            Debug.LogError("Fireball prefab is missing the Rigidbody2D component!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collision is with an object that should be affected by the spell
        // You might want to add more specific checks based on your game logic
        if (other.CompareTag("Enemy"))
        {
            // Apply damage to the enemy or perform other actions
            Debug.Log($"Fireball hit {other.gameObject.name}!");

            // Destroy the fireball
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player"))
        {
            // Destroy the fireball if it hits something other than the player or enemies
            Destroy(gameObject);
        }
    }
}