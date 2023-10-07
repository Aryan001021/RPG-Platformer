using UnityEngine;
//the collectable item script
public class ItemObject : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] ItemData itemData;//the scriptable object it contain
    [SerializeField] Vector2 velocity; 

    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = $"Item Object -{itemData.name}";
    }//work when script load // it give the item sprite same as in itemdata and make gameobject name same as in itemdata.   

    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;
        SetupVisuals();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            rb.velocity = velocity;
        }
    }

    public void PickupItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
