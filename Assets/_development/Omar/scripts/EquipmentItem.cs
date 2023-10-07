using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "EquipmentItem", menuName = "ScriptableObject/EquipmentItem")]
public class EquipmentItem : ScriptableObject
{
    public VisualTreeAsset UIElement;
    public int InventoryPosition;

}
