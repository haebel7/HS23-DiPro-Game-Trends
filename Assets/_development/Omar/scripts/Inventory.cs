using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public int size;
    public int numberOfSlotsPerRow;

    public Inventory(int size = 12, int numberOfSlotsPerRow = 3)
    {
        this.size = size;
        this.numberOfSlotsPerRow = numberOfSlotsPerRow;
    }

    public int getFirstFreeIndex()
    {
        Debug.Log("sdhjkrbgjhksbrg");
        Debug.Log(this.inventory.Count);
        int r = 0;
        int i = 0;
        foreach (var x in inventory)
        {
            Debug.Log(x.InventoryPosition);
            if(x.InventoryPosition > r)
            {
                r = x.InventoryPosition;
            }
            i++;
        }
        return r + 1;
    }

    public void dump()
    {
        foreach (var x in this.inventory)
        {
            Debug.Log(x.UIElement.ToString());
        }
    }

    public void addItem(InventoryItem item)
    {
        this.inventory.Add(item);
    }
}
