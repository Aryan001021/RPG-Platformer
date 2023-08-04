using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public int speed = 10;
    public float jumpForce = 5;
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
    
    #endregion
    void Awake()
    {
        Application.targetFrameRate = 60;
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
    }
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
        
    }
    public void SetVelocity(float _xVelocity,float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
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
}
