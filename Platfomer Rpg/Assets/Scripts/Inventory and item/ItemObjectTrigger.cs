using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject myItemObject=>GetComponentInParent<ItemObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<CharacterStats>().isDead) 
            {
                return;
            }
            Debug.Log("Picked Up Item");
            myItemObject.PickupItem();
        }//when we collide with it , it will destroy and that item is added in inventory
    }
}
