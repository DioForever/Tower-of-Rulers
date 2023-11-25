using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public bool isPickedUp;

    void Start()
    {
        isPickedUp = false;
    }
}
