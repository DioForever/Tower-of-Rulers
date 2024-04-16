using UnityEngine;
using Spells;

public class IceballScript : MonoBehaviour
{
    private float speed;
    private float distance;
    private float damage;
    private float slowDuration;
    private float mana;
    private SpellManager spellManager;
    private playerControl playercontrol;

    private void Awake()
    {
        
       

      
            // kde v listu je dany spell
            IceSpell iceballSpell = spellManager.spells[5] as IceSpell;

            // nastaveni rychlosti
            speed = iceballSpell.SpellSpeed;
            distance = iceballSpell.TravelDistance;
            mana = iceballSpell.Manacost;

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
           
            IceSpell iceballSpell = spellManager.spells[5] as IceSpell;

            damage = iceballSpell.Damage;
            slowDuration = iceballSpell.SlowDuration;

            //nemam enemy health pool
          

            
            iceballSpell.SlowTarget(other.gameObject, slowDuration);
           
            Destroy(gameObject);
        }

       
    }

   
}