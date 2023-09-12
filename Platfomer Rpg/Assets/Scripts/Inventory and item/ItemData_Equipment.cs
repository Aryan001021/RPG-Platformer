using UnityEngine;
public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
}
