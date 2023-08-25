using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration=.2f;
    public bool isBusy { get; private set; }
    [Header("Move Info")]
    public int speed = 10;
    public float jumpForce = 5;
    [Header("DashDir")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get;private set; }
    public SkillManager skill {  get; private set; }
 
 
    #region state
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallslideState wallslideState { get; private set; }
    public PlayerWallJumpState walljumpState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    
    #endregion
    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallslideState = new PlayerWallslideState(this, stateMachine, "WallSlide");
        walljumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine,"CounterAttack");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        skill = SkillManager.instance;
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
    }
    private void CheckForDashInput()
    {
        if(IsWallDetected())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)&&SkillManager.instance.dashSkill.CanUseSkill())
        {
            dashDir = Input.GetAxis("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDirection;
            }
            stateMachine.ChangeState(dashState);
            
        }
    }
    public void AnimationTrigger()=>stateMachine.currentState.AnimationFinishTrigger();
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy= true;
        yield return new WaitForSeconds(_seconds);
        isBusy= false;
        
    }
}
