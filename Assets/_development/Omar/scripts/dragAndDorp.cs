using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class dragAndDorp : MonoBehaviour
{
    private VisualElement UIroot;
    public Button addItem;
    public VisualTreeAsset item;
    public VisualElement itemEle;
    private VisualElement slot;
    private VisualElement UIinventory;
    private Inventory inventory = new Inventory();

    void OnEnable()
    {
        UIroot = GetComponent<UIDocument>().rootVisualElement;
        addItem = UIroot.Q<Button>("addItem");
        addItem.clicked += addItemToInventory;

        slot = UIroot.Q<VisualElement>("slot");
        UIinventory = UIroot.Q<VisualElement>("inventory");

        // VisualElement elemBlack = root.Q<VisualElement>("object");
        //VisualElement elemBlue = root.Q<VisualElement>("test");
        // DragAndDropManipulator manipulator = new(elemBlack);
        // DragAndDropManipulator manipulator2 = new(elemBlue);

        // moveToSlot(5, elemBlue);
    }

    private void addItemToInventory()
    {
        itemEle = item.Instantiate();
        itemEle.RegisterCallback<GeometryChangedEvent>(onItemInstantiate);
        UIinventory.Add(itemEle);
        InventoryItem newItem = new InventoryItem(itemEle, 0);
        inventory.addItem(newItem);
    }
    
    private void onItemInstantiate(GeometryChangedEvent evt)
    {
        itemEle.UnregisterCallback<GeometryChangedEvent>(onItemInstantiate);
        DragAndDropManipulator manipulator = new(itemEle);
        float slotWidth = slot.resolvedStyle.width;
        float itemMargin = slot.resolvedStyle.marginLeft;
        Debug.Log("Test");
        Debug.Log(inventory.getFirstFreeIndex());
        //itemEle.style.left = (inventory.getFirstFreeIndex() - 1) * slotWidth + itemMargin + (inventory.getFirstFreeIndex() - 1) * itemMargin * 2;
        //itemEle.style.top = itemMargin;
        //itemEle.style.position = new StyleEnum<Position>(Position.Absolute);
        //inventory.dump();
        
    }

    private void moveToSlot(int slotNumber, VisualElement elem)
    {
        Debug.Log(slot);
        float slotWidth = slot.resolvedStyle.width;
        Vector2 slotWorldSpace = elem.parent.LocalToWorld(elem.layout.position);
        elem.transform.position = UIroot.WorldToLocal(slotWorldSpace);
    }

}
