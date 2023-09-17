using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;
    private void OnValidate()
    {
        gameObject.name = "Equipment Slot- " + slotType.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);
        CleanupSlots();

    }
}
