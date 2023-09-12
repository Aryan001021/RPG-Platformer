using System;
[Serializable]
public class InventoryItem
{
    public ItemData data;//what item it is 
    public int stackSize; //how many it is 

    public InventoryItem(ItemData _data)//constructor
    {
        this.data = _data;
        AddStack();
    }
    public void AddStack()
    {
        stackSize++;
    }
    public void RemoveStack()
    {
        stackSize--;
    }
}
