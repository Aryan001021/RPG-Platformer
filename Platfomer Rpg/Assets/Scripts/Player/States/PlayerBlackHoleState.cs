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
        stateTimer = flyTime;//set up state timer
        rb.gravityScale = 0;//make player gravity 0

    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;//make player feel gravity again
        player.fX.MakeTransparent(false);//stop making him incisible
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }//make player fly
        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);//start droping him
            if (!skillUsed)//if we did not use the skill use it
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
