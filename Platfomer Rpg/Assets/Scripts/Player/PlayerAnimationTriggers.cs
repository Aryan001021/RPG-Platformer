using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player=>GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }//used by animation event to set and to change state in player and enemy state machine as in player he goes to idle
    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target= hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(_target);

            }
        }
    }//used by animation event ,when player play attack at the point of impact this is called
    public void ThrowSword()
    {
        SkillManager.instance.swordSkill.CreateSword();
    }//used by animation event thows the sword 
}
