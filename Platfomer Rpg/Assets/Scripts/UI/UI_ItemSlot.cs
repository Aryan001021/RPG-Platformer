using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField]Image image;
    [SerializeField]TextMeshProUGUI itemText;
    InventoryItem item;

    public void UpdateSlot(InventoryItem _item)
    {
        item= _item;
        image.color=Color.white;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
