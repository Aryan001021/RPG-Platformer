public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.4f;
        player.SetVelocity(-player.facingDirection * 5, player.jumpForce);//when player is sliding through wall and press jump then we give him backward velocity
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.airState);
        }//make him go to air state we he was not near ground
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }//if he was near ground then state change to idle
    }
}
