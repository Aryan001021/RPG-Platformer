using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;
    protected float xInput;
    protected string animBoolName;
    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine,string _animBoolName)
    {
        this.stateMachine = _playerStateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb=player.rb;
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
    public virtual void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        player.anim.SetFloat("YVelocity", rb.velocity.y);
    }
}
