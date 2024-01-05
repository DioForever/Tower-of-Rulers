using UnityEngine;
using Spells;

public class FireballScript : MonoBehaviour
{
    private float speed;
    private float distance;
    private float damage;
    private float burnDuration;

    private SpellManager spellManager;

    private void Start()
    {
        
       

      
            // kde v listu je dany spell
            FireSpell fireballSpell = spellManager.spells[5] as FireSpell;

            // nastaveni rychlosti
            speed = fireballSpell.SpellSpeed;
            distance = fireballSpell.TravelDistance;
            damage = fireballSpell.damage;
            burnDuration = fireballSpell.BurnDuration;

            // nastavit rychlost
            GetComponent<Rigidbody>().velocity = transform.forward * speed;

            //zničit po určité vzdálenosti
            Destroy(gameObject, distance / speed);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
           
            FireSpell fireballSpell = spellManager.spells[5] as FireSpell;

            //nemam enemy health pool

            
            if (fireballSpell != null)
            {
                fireballSpell.ApplyBurnEffect(other.gameObject, burnDuration);
            }

            // Destroy the Fireball on impact
            Destroy(gameObject);
        }

       
    }

   
}