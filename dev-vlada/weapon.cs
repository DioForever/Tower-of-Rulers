using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public bool isPickedUp;

    void Start()
    {
        isPickedUp = false;
    }
}