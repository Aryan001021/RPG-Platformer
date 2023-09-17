using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;//make it accessable from everywhere
    [Header("equipment Items that is currently equiped")]//for currently equip equipment
    public List<InventoryItem> equipment = new List<InventoryItem>();
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();
    [Header("Inventory for inventory")]//for equipment
    public List<InventoryItem> inventory = new List<InventoryItem>();//to see every item in inspector;
    public Dictionary<ItemData,InventoryItem> inventoryDictionary = new Dictionary<ItemData,InventoryItem>();//dictonary to keep track of items.
    [Header("Inventory for Material")]//for item
    public List<InventoryItem> stash = new List<InventoryItem>();//to see every item in inspector;
    public Dictionary<ItemData, InventoryItem> stashDictionary = new Dictionary<ItemData, InventoryItem>();//dictonary to keep track of items.

    [Header("Inventory UI")]
    [SerializeField] Transform inventorySlotParent;//all the equipment player currently have slot
    UI_ItemSlot[] inventoryItemSlots;

    [SerializeField] Transform stashSlotParent;//all the item player currently have slot
    UI_ItemSlot[] stashItemSlots;

    [SerializeField] Transform equipmentSlotParent;//the equiped equipment slots
    UI_EquipmentSlot[] equipmentSlots;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        inventoryItemSlots=inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlots= stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }
    private void UpdateSlotUI()
    {
        for(int i=0; i < equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlots[i].slotType)
                {
                    equipmentSlots[i].UpdateSlot(item.Value);
                }
            }
        }//fill the currently using equipment slot
        for(int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanupSlots(); 
        }//wmpty the current equipment have  slot

        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanupSlots();
        }//empty current item have slot

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }//fill the current equipment have  slot
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }//fill current item have slot

    }
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            AddToInventory(_item);
        }
        else if(_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }
        UpdateSlotUI();//add having item and equpment slot in UI
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }//|| if we have the item in dictionary then access it's value i.e.
         // InventoryItem.CS and add 1 in stack 
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }//if we dont have the item in dictonary add it in ductionary with its key as the itemdataSO and value as  inventoryitem.cs and pass item in its constructor
         // and add the InventoryItem.cs in inventoryitemlist so we can observe in inspector
    }//used for equipment
    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }//|| if we have the item in dictionary then access it's value i.e.
         // InventoryItem.CS and add 1 in stack 
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }//if we dont have the item in dictonary add it in ductionary with its key as the itemdataSO and value as  inventoryitem.cs and pass item in its constructor
         // and add the InventoryItem.cs in inventoryitemlist so we can observe in inspector
    }//used for items
    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as  ItemData_Equipment;//caste newEquipment to ItemDAta_Equipment type if it is already a type only
                                                                       //then it is casted otherwise it will give null eg. if _item is not of type
                                                                       //itemdata_equipment then the value will be null
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null; 
        foreach (KeyValuePair<ItemData_Equipment,InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }
        if(oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifier();
        RemoveItem(_item);
        UpdateSlotUI();
    }//used for equiping or wearing equipment

    public void UnequipItem(ItemData_Equipment itemToDelete)
    {
        if (equipmentDictionary.TryGetValue(itemToDelete, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToDelete);
            itemToDelete.RemoveModifier();
        }
    }//unequip the currently wearing armor if we change it

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item,out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            } //if we have only 1 left then remove the item from both from dictionary and lists
            else
            {
                value.RemoveStack();
            }//if we haave more than 1 item then subtract 1 from stack in that item's value in the dictionary 
        }
        if(stashDictionary.TryGetValue(_item,out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            } //if we have only 1 left then remove the item from both from dictionary and lists
            else
            {
                stashValue.RemoveStack();
            }//if we haave more than 1 item then subtract 1 from stack in that item's value in the dictionary 
        }

        UpdateSlotUI() ;
    }//remove item form equipment and item from dict. and list 

    public bool CanCraft(ItemData_Equipment _item,List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove=new List<InventoryItem>();
        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data,out InventoryItem stashValue))
            {
                //add this to used material
                if(stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("Not Enough Material");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("Not Enough Material");
                return false;
            }
        }
        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_item);
        Debug.Log("item added : " + _item.name);
        return true;
    }
     
}
