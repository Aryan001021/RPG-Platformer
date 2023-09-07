using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    float flyTime = .4f;
    bool skillUsed;
    float defaultGravity;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = rb.gravityScale;
        skillUsed = false;
        stateTimer =flyTime;
        rb.gravityScale = 0;

    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
        player.fX.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer>0)
        {
            rb.velocity = new Vector2(0, 15);
        }
        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);
            if (!skillUsed)
            {
                if (player.skill.blackHoleSkill.CanUseSkill())
                {
                    skillUsed = true;
                }
            }

        }
        if (player.skill.blackHoleSkill.SkillCompleted())
        {
            stateMachine.ChangeState(player.airState);
        }
        
    }
}
