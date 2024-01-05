using UnityEngine;
using Spells;

public class FireboomerangScript : MonoBehaviour
{
    private float speed;
    private float distance;
    private float damage;
    private float burnDuration;

    private SpellManager spellManager;
    private Vector3 originPosition; // Store the original position

    private void Start()
    {
       

        
            FireSpell fireballSpell = spellManager.spells[2] as FireSpell;

            speed = fireballSpell.SpellSpeed;
            distance = fireballSpell.TravelDistance;
            damage = fireballSpell.damage;
            burnDuration = fireballSpell.BurnDuration;

            originPosition = transform.position; // Store the original position

            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        
        else
        {
            Debug.LogError("SpellManager or FireSpell not found!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            FireSpell fireballSpell = spellManager.spells[2] as FireSpell;

            if (fireballSpell != null)
            {

                // tady damage pro enemy
                fireballSpell.ApplyBurnEffect(other.gameObject, burnDuration);
            }

            Destroy(gameObject);
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