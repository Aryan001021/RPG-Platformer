using UnityEngine;
//the collectable item script
public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;//the scriptable object it contain
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = $"Item Object -{ itemData.name}";
    }//work when script load // it give the item sprite same as in itemdata and make gameobject name same as in itemdata.
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>()!=null)
        {
            Inventory.instance.AddItem(itemData);
            Debug.Log("picked up item "+itemData.itemName);
            Destroy(gameObject);
        }//when we collide with it , it will destroy and that item is added in inventory
    }
}
