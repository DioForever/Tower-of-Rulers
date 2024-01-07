using UnityEngine;
using Spells;

public class FireboomerangScript : MonoBehaviour
{
    private float speed;
    private float distance;
    private float damage;
    private float burnDuration;
    private float mana;
    private SpellManager spellManager;

    private PlayerControl playercontrol;
    private Vector3 originPosition; // Store the original position

    private void Start()
    {        
            FireSpell fireballSpell = spellManager.spells[2] as FireSpell;

            speed = fireballSpell.SpellSpeed;
            distance = fireballSpell.TravelDistance;
            mana = fireballSpell.Manacost;

            playercontrol.mana = playercontrol.mana - mana;
           
            originPosition = transform.position; // Store the original position

            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        
            //zničit po určité vzdálenosti
            Destroy(gameObject, distance / speed);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            FireSpell fireballSpell = spellManager.spells[2] as FireSpell;
            damage = fireballSpell.damage;
            burnDuration = fireballSpell.BurnDuration;

        
            // tady damage pro enemy
            fireballSpell.ApplyBurnEffect(other.gameObject, burnDuration);
        }
    }

    private void Update()
    {
        
        if (Vector3.Distance(originPosition, transform.position) >= distance)
        {
            // Reverse the direction by multiplying velocity by -1
            GetComponent<Rigidbody>().velocity = -transform.forward * speed;
        }
    }
}