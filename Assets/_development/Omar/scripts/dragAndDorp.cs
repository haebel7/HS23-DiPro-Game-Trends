using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class dragAndDorp : MonoBehaviour
{
    public EquipmentInventory inventory;

    void OnEnable()
    {
        VisualElement UIroot = GetComponent<UIDocument>().rootVisualElement;
        VisualElement UIinventory = UIroot.Q<VisualElement>("inventory");
        inventory.initEquipmentInventory(UIinventory);
    }
}
