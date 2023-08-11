using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public bool isBusy { get; private set; }
    [Header("Move Info")]
    public int speed = 10;
    public float jumpForce = 5;
    [Header("DashDir")]
    [SerializeField] float dashCoolDown;
    private float dashCoolTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get;private set; }
    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask whatIsGround;
    public int facingDirection { get; private set; } = 1;
    private bool facingRight = true;
    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion
    #region state
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallslideState wallslideState { get; private set; }
    public PlayerWallJumpState walljumpState { get; private set; }

    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    
    #endregion
    void Awake()
    {
        Application.targetFrameRate = 60;
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallslideState = new PlayerWallslideState(this, stateMachine, "WallSlide");
        walljumpState = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
    }
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
    }
    void Update()
    {
        stateMachine.currentState.Update();
        CheckForDashInput();
    }
    private void CheckForDashInput()
    {
        if(IsWallDetected())
        {
            return;
        }
        dashCoolTimer-= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift)&&dashCoolTimer<0)
        {
            dashCoolTimer = dashCoolDown;
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
    #region Velocity
    public void ZeroVelocity() => rb.velocity = Vector2.zero;
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion 
    #region collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    #endregion
    #region flip
    public void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if(_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
}
