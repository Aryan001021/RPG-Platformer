using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    protected int comboCounter;
    private float lastTimeAttack;
    private float comboWindow=2f;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (comboCounter > 2 || Time.time >= lastTimeAttack + comboWindow)
        {
            comboCounter = 0;
        }
        player.anim.SetInteger("ComboCounter", comboCounter);
        float attackDirection = player.facingDirection;
        if (attackDirection != player.facingDirection)
        {
            attackDirection = xInput;
        }
        player.SetVelocity(player.attackMovement[comboCounter].x *attackDirection, player.attackMovement[comboCounter].y);
        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastTimeAttack=Time.time;
        player.StartCoroutine("BusyFor", .15f);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            rb.velocity=Vector2.zero;
        }
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
