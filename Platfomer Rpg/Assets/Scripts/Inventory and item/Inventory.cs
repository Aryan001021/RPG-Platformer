using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;//make it accessable from everywhere
    [Header("Inventory for equipment")]
    public List<InventoryItem> inventory = new List<InventoryItem>();//to see every item in inspector;
    public Dictionary<ItemData,InventoryItem> inventoryDictionary = new Dictionary<ItemData,InventoryItem>();//dictonary to keep track of items.
    [Header("Inventory for Material")]
    public List<InventoryItem> stash = new List<InventoryItem>();//to see every item in inspector;
    public Dictionary<ItemData, InventoryItem> stashDictionary = new Dictionary<ItemData, InventoryItem>();//dictonary to keep track of items.

    [Header("Inventory UI")]
    [SerializeField] Transform inventorySlotParent;
    UI_ItemSlot[] inventoryItemSlots;
    [SerializeField] Transform stashSlotParent;
    UI_ItemSlot[] stashItemSlots;
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
    }
    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }
       
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
        UpdateSlotUI();//update slot in UI
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
    }
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
    }

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
    }
     
}
