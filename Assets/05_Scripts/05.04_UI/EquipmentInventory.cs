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
    private List<VisualElement> temp = new List<VisualElement>();

    public void initEquipmentInventory(VisualElement UIroot)
    {
        this.UIroot = UIroot;
        this.UIslot = this.UIroot.Q<VisualElement>("slot");
        this.UIinventory = this.UIroot.Q<VisualElement>("inventory");
        for (int i = 0; i < this.inventory.Count; i++)
        {
            addEquipmentItemToUIInventory(this.inventory[i]);
        }
    }



    public void addEquipmentItemToUIInventory(EquipmentItem item)
    {
        newItemUI = item.UIElement.Instantiate();
        temp.Add(newItemUI);
        UIinventory.Add(newItemUI);
        newItemUI.RegisterCallback<GeometryChangedEvent>(onNewItemUILoad);


        void onNewItemUILoad(GeometryChangedEvent evt)
        {
            VisualElement newItemUITemp = temp[0];
            temp.RemoveAt(0);
            newItemUITemp.UnregisterCallback<GeometryChangedEvent>(onNewItemUILoad);
            DragAndDropManipulator manipulator = new(newItemUITemp, item, this);
            float slotWidth = this.UIslot.resolvedStyle.width;
            float itemWidth = newItemUITemp.Q<VisualElement>("item").resolvedStyle.width;
            float itemMargin = this.UIslot.resolvedStyle.marginLeft;
            float slotTranslate = (slotWidth - itemWidth) / 2;
            newItemUITemp.style.translate = new StyleTranslate(
                new Translate(
                    new Length(
                        (item.InventoryPosition % this.numberOfSlotsPerRow * slotWidth + item.InventoryPosition % this.numberOfSlotsPerRow * itemMargin * 2),
                         LengthUnit.Pixel
                    ),
                    new Length(
                         (float)Math.Floor((float)(item.InventoryPosition / this.numberOfSlotsPerRow)) * slotWidth + (float)Math.Floor((float)(item.InventoryPosition / this.numberOfSlotsPerRow)) * slotTranslate,
                         LengthUnit.Pixel
                    )
                )
            );
            newItemUITemp.style.top = itemMargin + slotTranslate;
            newItemUITemp.style.left = itemMargin + slotTranslate;
            newItemUITemp.style.position = new StyleEnum<Position>(Position.Absolute);
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

    public void moveItemFromTo(int oldIndex, int newIndex)
    {
        foreach (var item in this.inventory)
        {
            if(item.InventoryPosition == oldIndex)
            {
                item.InventoryPosition = newIndex;
                break;
            }
        }
    }
    public bool placeIsFree(int position)
    {
        foreach (var item in this.inventory)
        {
            if (item.InventoryPosition == position)
            {
                return false;
            }
        }
        return true;
    }
}
