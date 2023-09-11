using UnityEngine;
//this script is a base base class and it need to be inherited by all player state
public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;
    protected float xInput;
    protected float yInput;
    protected string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;
    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName)
    {
        this.stateMachine = _playerStateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }//constructor
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }//when ever we enter a state it run once , it turns of current state animation
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }//state exit state ,it set current animation to false
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("YVelocity", rb.velocity.y);
    }//states update loop
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
