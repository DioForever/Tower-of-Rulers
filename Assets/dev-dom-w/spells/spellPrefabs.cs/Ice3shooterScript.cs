using UnityEngine;
using Spells;

public class Ice3shooterScript : MonoBehaviour
{
    private float speed;
    private float distance;
    private float damage;
    

    private SpellManager spellManager;

    private void Start()
    {
       
            
            IceSpell IceballSpell = spellManager.spells[3] as IceSpell;

           
            speed = IceballSpell.SpellSpeed;
            distance = IceballSpell.TravelDistance;

            
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
        IceSpell IceballSpell = spellManager.spells[3] as IceSpell;

        damage = IceballSpell.damage;
        

        if (IceballSpell != null)
        {
            //nastavit slow efekt
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
