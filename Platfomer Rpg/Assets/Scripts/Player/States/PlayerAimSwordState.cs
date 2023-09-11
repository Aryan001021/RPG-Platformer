using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.swordSkill.DotsActive(true);
    }//start drawing dots according to mouse position

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .2f);
    }//make player busy

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();//make player stop to aim
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idleState);
        }//if player stop pressing the button then idle state
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//getting mouse position
        if (player.transform.position.x > mousePosition.x && player.facingDirection == 1)
        {
            player.Flip();
        }//flipping player according to mouse position
        else if (player.transform.position.x < mousePosition.x && player.facingDirection == -1)
        {
            player.Flip();
        }//flipping player according to mouse position
    }
}
