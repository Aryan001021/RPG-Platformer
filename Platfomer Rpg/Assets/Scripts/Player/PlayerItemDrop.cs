using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player Item Drop")]
    [Range(0,100)][SerializeField] float chanceToLoseItems;
    public override void GenerateDrop()
    {
         List<InventoryItem> currentEquipment=Inventory.instance.GetEquipmentList();
        List<InventoryItem> itemToUnequipment = new List<InventoryItem>();
        foreach (InventoryItem item in currentEquipment)
        {
            if (Random.Range(0, 100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemToUnequipment.Add(item);
            }
        }
        for (int i = 0; i < itemToUnequipment.Count; i++)
        {
            Inventory.instance.UnequipItem(itemToUnequipment[i].data as ItemData_Equipment);
        }
    }
    
}
