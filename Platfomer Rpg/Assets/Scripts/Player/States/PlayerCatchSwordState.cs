using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;
        if (player.transform.position.x > sword.position.x && player.facingDirection == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < sword.position.x && player.facingDirection == -1)
        {
            player.Flip();
        }
        rb.velocity=new Vector2(-player.swordReturnImpact *player.facingDirection,rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
}
