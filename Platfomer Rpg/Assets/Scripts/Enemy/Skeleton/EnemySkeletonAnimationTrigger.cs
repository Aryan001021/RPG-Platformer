using UnityEngine;

public class EnemySkeletonAnimationTrigger : MonoBehaviour
{
    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();
    public void AnimationTrigger()
    {
        enemy.AnimationFinishedTrigger();
    }//animation trigger for indicating that animation is completed
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);

            }
        }
    }//hit player
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();//to give player visual queue that enemy is about to attack
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();//remove counter visual

}
