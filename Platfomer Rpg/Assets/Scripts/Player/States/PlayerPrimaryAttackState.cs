using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }//the animation index we are on
    private float lastTimeAttack;
    private float comboWindow = 2f;//if player dont press in 2 sec then the next attack will be 1st animation
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;
        if (comboCounter > 2 || Time.time >= lastTimeAttack + comboWindow)
        {
            comboCounter = 0;
        }//if player keep on attacking after the whole combo then it reset to zero and make it start from 1 and if the time of attack is more
         //than lasttimeattact + combocounter then it is set to zero
        player.anim.SetInteger("ComboCounter", comboCounter);
        float attackDirection = player.facingDirection;
        if (attackDirection != player.facingDirection)
        {
            attackDirection = xInput;
        }//change attact direction on x input so player can do current attact animation to other side if he wishes to
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);//make player move forward according
                                                                                                                           //to the value set on player and
                                                                                                                           //current combo
        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastTimeAttack = Time.time;
        player.StartCoroutine("BusyFor", .15f);//make player busy
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            rb.velocity = Vector2.zero;//make player attack velocity do not apply after a time
        }
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
