using UnityEngine;

public interface IWeapon
{
    void Attack();
}
public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float arrowSpeed = 10f;
    [SerializeField] private int arrowDamage = 10;

    public void Attack()
    {
        FireArrow();
    }

    private void FireArrow()
    {
        GameObject arrow = new GameObject("Arrow");
        arrow.transform.position = firePoint.position;
        arrow.transform.rotation = Quaternion.identity;

        Rigidbody2D arrowRb = arrow.AddComponent<Rigidbody2D>();
        arrowRb.velocity = firePoint.right * arrowSpeed;

        ArrowScript arrowScript = arrow.AddComponent<ArrowScript>();
        arrowScript.SetDamage(arrowDamage);
        arrowScript.SetEnemyLayer(enemyLayer);
    }
}

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private LayerMask enemyLayer;

    public void Attack()
    {
        PerformMeleeAttack();
    }

    private void PerformMeleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, meleeRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(10); // You can adjust the damage amount
            }
        }
    }
}


