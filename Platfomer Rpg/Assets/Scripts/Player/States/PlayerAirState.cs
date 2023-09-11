public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallslideState);
        }//wall detected ahead=>wallslide state
        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * .7f * xInput, rb.velocity.y);
        }//make player velocity slow in air
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }//if ground detected=>idle
    }
}
