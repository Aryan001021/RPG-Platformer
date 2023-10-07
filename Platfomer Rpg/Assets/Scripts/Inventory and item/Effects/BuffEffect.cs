using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critDamage,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    lightningDamage,
    iceDamage
}
[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/item effect/Buff Effect")]

public class BuffEffect : ItemEffect
{
    PlayerStats stats;
    [SerializeField] StatType buffType;
    [SerializeField] int buffAmount;
    [SerializeField]float buffDuration;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats=PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatsBy(buffAmount, buffDuration, StatToModify());
    }
    private Stats StatToModify()
    {
        switch (buffType)
        {
            case StatType.strength:return stats.strength;
            case StatType.agility:return stats.agility;
            case StatType.intelligence:return stats.intelligence;
            case StatType.vitality:return stats.vitality;
            case StatType.damage:return stats.damage;
            case StatType.critChance:return stats.critChance;
            case StatType.critDamage:return stats.critDamage;
            case StatType.maxHealth:return stats.maxHealth;
            case StatType.armor:return stats.armor;
            case StatType.evasion:return stats.evasion;
            case StatType.magicResistance:return stats.magicResistance;
            case StatType.fireDamage:return stats.fireDamage;
            case StatType.lightningDamage:return stats.lightningDamage;
            case StatType.iceDamage:return stats.iceDamage;
            default: return null;
        }

    }
}
