using UnityEngine;
using Spells;

public class FirelanceScript : MonoBehaviour
{
    private float speed;
    private float distance;
    private float damage;
    private float burnDuration;
    private float mana;
    private SpellManager spellManager;
    private playerControl playercontrol;

    private void Awake()
    {
        
       

        
            // kde v listu je dany spell
            FireSpell fireballSpell = spellManager.spells[1] as FireSpell;

            // nastaveni values
            speed = fireballSpell.SpellSpeed;
            distance = fireballSpell.TravelDistance;
            mana = fireballSpell.Manacost;

            playercontrol.mana = playercontrol.mana - mana;
           

            // nastavit rychlost
            GetComponent<Rigidbody>().velocity = transform.forward * speed;

            //zničit po určité vzdálenosti
            Destroy(gameObject, distance / speed);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
           
            FireSpell fireballSpell = spellManager.spells[1] as FireSpell;
            damage = fireballSpell.damage;
            burnDuration = fireballSpell.BurnDuration;

            //nemam enemy health pool

            
          
            
                fireballSpell.ApplyBurnEffect(other.gameObject, burnDuration);
            

            // Destroy the Fireball on impact
            Destroy(gameObject);
        }

       
    }

   
}