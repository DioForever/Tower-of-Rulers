using UnityEngine;
using Spells;

public class IceballScript : MonoBehaviour
{
    private float speed;
    private float distance;
    private float damage;
    private float freezeDuration;
    private float slowDuration;
    

    private SpellManager spellManager;

    private void Start()
    {
        
       

      
            // kde v listu je dany spell
            IceSpell iceballSpell = spellManager.spells[5] as IceSpell;

            // nastaveni rychlosti
            speed = iceballSpell.SpellSpeed;
            distance = iceballSpell.TravelDistance;
           
            

            // nastavit rychlost
            GetComponent<Rigidbody>().velocity = transform.forward * speed;

            //zničit po určité vzdálenosti
            Destroy(gameObject, distance / speed);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
           
            IceSpell iceballSpell = spellManager.spells[5] as IceSpell;

            damage = iceballSpell.Damage;
            freezeDuration = iceballSpell.FreezeDuration;
            slowDuration = iceballSpell.SlowDuration;

            //nemam enemy health pool
            //nemam debuffy

            
            if (iceballSpell != null)
            {
                //dat sem freeze a slow effect
            }

            // Destroy the Fireball on impact
            Destroy(gameObject);
        }

       
    }

   
}