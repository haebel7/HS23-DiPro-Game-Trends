using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "EquipmentInventory", menuName = "ScriptableObject/EquipmentInventory")]
public class EquipmentInventory : ScriptableObject
{
    public List<EquipmentItem> inventory = new List<EquipmentItem>();
    public int size;
    public int numberOfSlotsPerRow;

    private GameObject inventoryUI;
    private VisualElement UIroot;
    private VisualElement UIslot;
    private VisualElement UIinventory;
    private VisualElement newItemUI;

    public void initEquipmentInventory(VisualElement UIroot)
    {
        this.UIroot = UIroot;
        this.UIslot = this.UIroot.Q<VisualElement>("slot");
        this.UIinventory = UIroot.Q<VisualElement>("inventory");
        for (int i = 0; i < this.inventory.Count; i++)
        {
            addEquipmentItemToUIInventory(this.inventory[i]);
        }
    }

    public void addEquipmentItemToUIInventory(EquipmentItem item)
    {
        newItemUI = item.UIElement.Instantiate();
        UIinventory.Add(newItemUI);
        newItemUI.RegisterCallback<GeometryChangedEvent>(onNewItemUILoad);

        void onNewItemUILoad(GeometryChangedEvent evt)
        {
            newItemUI.UnregisterCallback<GeometryChangedEvent>(onNewItemUILoad);
            DragAndDropManipulator manipulator = new(newItemUI);
            float slotWidth = this.UIslot.resolvedStyle.width;
            float itemWidth = newItemUI.Q<VisualElement>("item").resolvedStyle.width;
            float itemMargin = this.UIslot.resolvedStyle.marginLeft;
            float slotTranslate = (slotWidth - itemWidth) / 2;
            newItemUI.style.translate = new StyleTranslate(
                new Translate(
                    new Length(
                        (item.InventoryPosition * slotWidth + item.InventoryPosition * itemMargin * 2),
                         LengthUnit.Pixel
                    ),
                    new Length(
                        0,
                         LengthUnit.Pixel
                    )
                )
            );
            newItemUI.style.top = itemMargin + slotTranslate;
            newItemUI.style.left = itemMargin + slotTranslate;
            newItemUI.style.position = new StyleEnum<Position>(Position.Absolute);
            //this.dump();
        }
    }

    public int getFirstFreeIndex()
    {
        for (int i = 0; i < this.size; i++)
        {
            bool r = false;
            foreach (var x in this.inventory)
            {
                if (x.InventoryPosition == i)
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

    public void addItem(EquipmentItem item)
    {
        this.inventory.Add(item);
    }
}
