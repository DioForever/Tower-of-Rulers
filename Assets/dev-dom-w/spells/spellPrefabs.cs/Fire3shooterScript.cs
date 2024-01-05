using UnityEngine;
using Spells;

public class Fire3shooterScript : MonoBehaviour
{
    private float speed;
    private float distance;
    private float damage;
    private float burnDuration;

    private SpellManager spellManager;

    private void Start()
    {
       
            
            FireSpell fireballSpell = spellManager.spells[3] as FireSpell;

           
            speed = fireballSpell.SpellSpeed;
            distance = fireballSpell.TravelDistance;

            
            GetComponent<Rigidbody>().velocity = transform.forward * speed;

            // Instantiate projectiles at 45 degrees left and right
            SpawnProjectile(Vector3.left);
            SpawnProjectile(Vector3.right);

            
            Destroy(gameObject, distance / speed);
        
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Enemy"))
    {
        FireSpell fireballSpell = spellManager.spells[3] as FireSpell;

        damage = fireballSpell.damage;
        burnDuration = fireballSpell.BurnDuration;

        if (fireballSpell != null)
        {
            fireballSpell.ApplyBurnEffect(other.gameObject, burnDuration);
        }

        // Destroy the projectile that collided with the enemy
        Destroy(gameObject);
    }
}

    private void SpawnProjectile(Vector3 direction)
    {
        // rotace 45 stupňů
        GameObject projectile = Instantiate(gameObject, transform.position, transform.rotation);

       
        projectile.GetComponent<Rigidbody>().velocity = (transform.forward + direction).normalized * speed;

        
        Destroy(projectile, distance / speed);
    }
}
