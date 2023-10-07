 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemy Effect", menuName = "Data/item effect/Freeze Emeny Effect")]
public class FreezeEnemyEffect : ItemEffect
{
    [SerializeField] float duration;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStat=PlayerManager.instance.player.GetComponent<PlayerStats>();
        if(playerStat.currentHealth >playerStat.GetMaxHealth()*0.1f)
        {
            return; 
        }
        if (Inventory.instance.UseArmor())
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemyPosition.position, 2);
            foreach (var hit in colliders)
            {
                hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
            }
        } 
    }
}
