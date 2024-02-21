using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    private List<Item> itemList;

    public Inventory () 
    {
        itemList = new List<Item>();

        Debug.Log("Inventory");
    }

}
