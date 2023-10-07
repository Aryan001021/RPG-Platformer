
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler
{
    [SerializeField]Image image;
    [SerializeField]TextMeshProUGUI itemText;
    public InventoryItem item;

    public void CleanupSlots()
    {
        item = null;
        image.color = Color.clear;
        image.sprite = null;  
        itemText.text = "";
    }//make slot empty

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }
    }//when player click on item

    public void UpdateSlot(InventoryItem _item)
    {
        item= _item;
        image.color = Color.white;
        if (item != null)
        {
            image.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
        if (item == null)
        {
        }
    }
}
