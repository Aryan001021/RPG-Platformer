public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.cloneSkill.CreateCloneOnDashStart();//if  ability open to creater clone on start of dash
        stateTimer = player.dashDuration;//setting up state timer
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.cloneSkill.CreateCloneOnDashEnd();//if  ability open to creater clone on end of dash
        player.SetVelocity(0, rb.velocity.y);//set player x velocity to zero
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);//giving dash velocity
        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallslideState);
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
