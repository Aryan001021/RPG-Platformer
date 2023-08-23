using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    EnemySkeleton enemy;
    GameObject player;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.gameObject;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (Canattack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            if (stateTimer < 0|| Vector2.Distance(player.transform.position,enemy.transform.position)>15)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
        if (player.transform.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.transform.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }
    private bool Canattack()
    {
        if(Time.time>=enemy.lastTimeAttacked+enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        else { 
        return false;
        }
    }

}
