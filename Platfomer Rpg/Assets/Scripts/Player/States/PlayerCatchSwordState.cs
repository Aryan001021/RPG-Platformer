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
            player.Flip();//if sword on opposite direction then player will turn
        }
        else if (player.transform.position.x < sword.position.x && player.facingDirection == -1)
        {
            player.Flip();//if sword on opposite direction then player will turn
        }
        rb.velocity = new Vector2(-player.swordReturnImpact * player.facingDirection, rb.velocity.y);//when player catch the sword he well feel the inertia 
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);//player got busy 
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }//make player goes back to idle

    }
}
