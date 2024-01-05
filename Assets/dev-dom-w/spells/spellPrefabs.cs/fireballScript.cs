using UnityEngine;
using Spells;

public class FireballScript : MonoBehaviour
{
    private float speeds;
    private float distances;
    private float damages;
    private float burnDurations;

    private SpellManager mySpellManager;

    private void Start()
    {
        
       

      
        // kde v listu je dany spell
        FireSpell fireballSpell = mySpellManager.spells[0] as FireSpell;

        // nastaveni rychlosti
        speeds = fireballSpell.SpellSpeed;
        distances = fireballSpell.TravelDistance;
        damages = fireballSpell.damage;
        burnDurations = fireballSpell.BurnDuration;

        // nastavit rychlost
        GetComponent<Rigidbody>().velocity = transform.forward * speeds;

        //zničit po určité vzdálenosti
        Destroy(gameObject, distances / speeds);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
           
            FireSpell fireballSpell = mySpellManager.spells[0] as FireSpell;

            //nemam enemy health pool

            
            if (fireballSpell != null)
            {
                fireballSpell.ApplyBurnEffect(other.gameObject, burnDurations);
            }

            // Destroy the Fireball on impact
            Destroy(gameObject);
        }

       
    }

   
}