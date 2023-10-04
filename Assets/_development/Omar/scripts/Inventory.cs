using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
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
        for (int i = 0; i < this.size; i++)
        {
            bool r = false;
            foreach (var x in this.inventory)
            {
                if(x.InventoryPosition == i)
                {
                    r = true;
                    break;
                }
            }
            if (!r)
            {
                return i;
            }
        }
        return this.size;
    }

    public void dump()
    {
        Debug.Log("---------------------");
        foreach (var x in this.inventory)
        {
            Debug.Log("InventoryPosition: " + x.InventoryPosition.ToString());
            Debug.Log("UIElement: " + x.UIElement.ToString());
        }
        Debug.Log("---------------------");
    }

    public void addItem(InventoryItem item)
    {
        this.inventory.Add(item);
    }
}
*/