using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private float damage;
    private LayerMask enemyLayer;

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public void SetEnemyLayer(LayerMask newEnemyLayer)
    {
        enemyLayer = newEnemyLayer;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is an enemy
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            // Assuming the enemy has a component that handles taking damage
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Deal damage to the enemy
                damageable.TakeDamage(damage);
            }

            // Destroy the arrow on hit
            Destroy(gameObject);
        }
    }
}
