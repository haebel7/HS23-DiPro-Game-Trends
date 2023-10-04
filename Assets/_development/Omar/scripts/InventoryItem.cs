using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryItem
{
    public VisualElement UIElement;
    public int InventoryPosition;

    public InventoryItem(VisualElement UIElement, int InventoryPosition)
    {
        this.UIElement = UIElement;
        this.InventoryPosition = InventoryPosition;
    }

}
