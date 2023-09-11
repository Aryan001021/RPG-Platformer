using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);//give player y velocity
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (rb.velocity.y < 0)//if player got y velocity then change player state to air state
        {
            stateMachine.ChangeState(player.airState);
        }
        base.Update();
    }
}
