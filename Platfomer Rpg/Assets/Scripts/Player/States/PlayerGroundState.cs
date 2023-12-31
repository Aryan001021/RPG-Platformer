using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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
        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(player.blackHoleState);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        {
            stateMachine.ChangeState(player.aimSwordState);
        }
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttack);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.counterAttackState);
        }
    }//checking input of player if he is on ground and if he  is not then make him go airstate
    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }//make the thrown sword come back
}
