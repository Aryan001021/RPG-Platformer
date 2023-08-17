using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTrigger : MonoBehaviour
{
   private EnemySkeleton enemy=>GetComponentInParent<EnemySkeleton>();
    public void AnimationTrigger()
    {
        enemy.AnimationFinishedTrigger();
    }
}
