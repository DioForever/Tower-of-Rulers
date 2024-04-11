using UnityEngine;
using Spells;

public class IceFreezeScript : MonoBehaviour
{
    private float speed;
    private float distance;
    private float damage;
    private float freezeDuration;
    private float mana;
    private SpellManager spellManager;
    private playerControl playercontrol;

    private void Start()
    {
        IceSpell iceballSpell = spellManager.spells[8] as IceSpell;

        speed = iceballSpell.SpellSpeed;
        distance = iceballSpell.TravelDistance;
        mana = iceballSpell.Manacost;

        playercontrol.mana = playercontrol.mana - mana;
       

        // Instantiate 8 projectiles in a circular pattern
        SpawnProjectiles(8);

        Destroy(gameObject, distance / speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IceSpell iceballSpell = spellManager.spells[8] as IceSpell;

            damage = iceballSpell.Damage;
            freezeDuration = iceballSpell.FreezeDuration;
            


            iceballSpell.FreezeTarget(other.gameObject, freezeDuration);

           
        }
    }

    private void SpawnProjectiles(int count)
    {
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            //úhel projektilu
            float angle = i * angleStep;

            // radiány
            float radianAngle = Mathf.Deg2Rad * angle;

            //dát to do kruhu
            float spawnX = transform.position.x + Mathf.Cos(radianAngle) * 1f; // Adjust 1f for the radius
            float spawnZ = transform.position.z + Mathf.Sin(radianAngle) * 1f;

            Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, spawnZ);

            
            GameObject projectile = Instantiate(gameObject, spawnPosition, Quaternion.identity);

            
            projectile.GetComponent<Rigidbody>().velocity = (spawnPosition - transform.position).normalized * speed;

            
            Destroy(projectile, distance / speed);
        }
    }
}