using System.Collections.Generic;
using UnityEditor;
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
    public EquipmentType equipmentType;//what wearable is it
    public float itemCooldown;
    public ItemEffect[] itemEffects;
    [Header("Major Stats")]
    public int strength;  //1 point  increase damage by 1 and crit power by 1%
    public int agility;   // 1 point increse evasion  by 1% and crit chance by 1%
    public int intelligence;  //1 point increase magic damage by 1 and magic resistance by 3
    public int vitality;// 1 point  increase hp by 3 or 5

    [Header("Offensive Stats")]
    public int damage;//the  base damage that hit the opponent
    public int critChance;//the percentage of time crit damage will apply
    public int critDamage;//the crit damage that mutiplied to the damage when crit chance is in favour

    [Header("Defensive Stats")]
    public int maxHealth;//max Hp
    public int armor;//added to the hp by armor and such
    public int evasion;//chance to evade the attack
    public int magicResistance;//defence against magic

    [Header("Magic Stats")]
    public int fireDamage;//fire attack damage
    public int lightningDamage;//lightning attack damage
    public int iceDamage;//freeze attack damage

    [Header("Craft Requirement")]
    public List<InventoryItem> craftingMaterials;
    
    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }
    public void AddModifier()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critDamage.AddModifier(critDamage);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
        playerStats.iceDamage.AddModifier(iceDamage);

    }//modifier adder to player on equiping this equipment
    public void RemoveModifier()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critDamage.RemoveModifier(critDamage);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);

    }//remove modifier from player on unquiping this equipment
}
