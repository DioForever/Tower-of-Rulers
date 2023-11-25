using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))  // Assuming the left mouse button is used for shooting
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Bullet is missing Rigidbody2D component.");
        }
    }
}