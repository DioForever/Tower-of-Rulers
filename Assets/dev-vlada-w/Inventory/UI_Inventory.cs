using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }
}
