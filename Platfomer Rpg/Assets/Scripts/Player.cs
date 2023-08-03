using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public int speed = 10;
    public float jumpForce=5;
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
    }
}
