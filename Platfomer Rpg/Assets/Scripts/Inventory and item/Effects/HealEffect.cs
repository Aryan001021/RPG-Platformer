using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/item effect/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] float healPercentage;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats= PlayerManager.instance.player.GetComponent<PlayerStats>();
        int healAmount=Mathf.RoundToInt(playerStats.GetMaxHealth()*healPercentage);
        playerStats.IncreaseHealthBy(healAmount);
    }
}
