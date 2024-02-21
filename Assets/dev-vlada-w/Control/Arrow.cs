using UnityEngine;
public interface IDamageable
{
    // Property to get and set the health of the object.
    int Health { get; set; }

    // Method to apply damage to the object.
    void Damage(int damageAmount);

    // Optional: Method to heal the object.
    void Heal(int healAmount);
}


public class ArrowScript : MonoBehaviour, IDamageable
{
    public int Health { get; set; }

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;

        // Trigger the OnTakeDamage event.
        // OnTakeDamage?.Invoke(damageAmount);

        if (Health <= 0)
        {
            // Trigger the OnDeath event.
            // OnDeath?.Invoke();
        }
    }

    public void Heal(int healAmount)
    {
        if (Health <= 0)
        {
            // Trigger the OnDeath event.
            // OnDeath?.Invoke();
        }else {
            Health += healAmount;
        }
    }


    private int damage;
    private LayerMask enemyLayer;

    public void SetDamage(int newDamage)
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
                damageable.Damage(damage);
            }

            // Destroy the arrow on hit
            Destroy(gameObject);
        }
    }
}
