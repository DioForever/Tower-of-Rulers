using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damageAmount = 10f; // Amount of damage dealt to enemy

    public void PerformAttack()
    {
        // Perform attack logic here
        Debug.Log("Performing attack with damage: " + damageAmount);
    }
}
