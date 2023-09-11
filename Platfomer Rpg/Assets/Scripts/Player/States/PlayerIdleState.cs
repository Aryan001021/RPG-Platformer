using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (xInput == player.facingDirection && player.IsWallDetected())
        {
            return;
        }//this make player to not change to other animation if he is trying to move towards a wall 
        if (xInput != 0 && player.isBusy == false)
        {
            stateMachine.ChangeState(player.moveState);
        }//this make him go to moving state
    }
}


