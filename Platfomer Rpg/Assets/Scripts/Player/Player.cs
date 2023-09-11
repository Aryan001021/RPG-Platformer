using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Details")]
    public Vector2[] attackMovement;//list of vector so player can move forward or backward  while attacking according to the ongoing animation 
    public float counterAttackDuration = .2f;
    public bool isBusy { get; private set; }
    [Header("Move Info")]
    public float moveSpeed = 10;
    public float jumpForce = 5;
    public float swordReturnImpact;//when player catch the sword he throw he gets back it the amount of inertia he feel 
    float defaultJumpSpeed;
    float defaultMoveSpeed;
    [Header("DashDir")]
    public float dashSpeed;
    public float dashDuration;
    float defaultDashSpeed;
    public float dashDir { get; private set; }
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }//sword that is instantiated for throw

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
    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerDeathState deathState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;//set FPS of game
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallslideState = new PlayerWallslideState(this, stateMachine, "WallSlide");
        walljumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");
        deathState = new PlayerDeathState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);//pass the initial state of statemachine
        skill = SkillManager.instance;
        defaultJumpSpeed = jumpForce;
        defaultMoveSpeed = moveSpeed;
        defaultDashSpeed = dashSpeed;
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();//run update function of player's currentstate
        CheckForDashInput();
        if (Input.GetKeyDown(KeyCode.F))
        {
            skill.crystalSkill.CanUseSkill();
        }
    }
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowDuration);
        dashSpeed = dashSpeed * (1 - _slowDuration);
        anim.speed = anim.speed * (1 - _slowPercentage);
        Invoke("ReturnToDefaultSpeed", _slowDuration);

    }//slow playerr by given amount according to the effect of current magic player is under eg freeze
    protected override void ReturnToDefaultSpeed()
    {
        base.ReturnToDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpSpeed;
        dashSpeed = defaultDashSpeed;
    }//remove the maigic effect
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }//if the sword we throw is instantiaed here
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }//catch the thrown sword and destroys it

    private void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dashSkill.CanUseSkill())
        {
            dashDir = Input.GetAxis("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDirection;
            }
            stateMachine.ChangeState(dashState);

        }
    }//to do dash as dash can be done at any state thats why its here
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();//when player dies the death state call this to play the trigger
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }//a coroutine to make player busy bool on it is set while attacking
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }// when player die it calls and it changes player current state to death state
}
