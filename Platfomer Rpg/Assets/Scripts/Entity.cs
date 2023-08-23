using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Check")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("KnockBack Info")]
    [SerializeField] protected Vector2 knockBackDir;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnocked;

    public int facingDirection { get; private set; } = 1;
    protected bool facingRight = true;
    #region Component

    public EntityFX fX { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fX = GetComponent<EntityFX>();
    }
    protected virtual void Update()
    {

    }
    public virtual void Damage()
    {
        Debug.Log(this.gameObject.name);
        fX.StartCoroutine("FlashFX");
        StartCoroutine(HitKnockBack());
    }
    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockBackDir.x * -facingDirection, knockBackDir.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
    }
    #region Velocity
    public void SetZeroVelocity() 
    {
        if(isKnocked)
        {
            return;
        }
        rb.velocity = Vector2.zero;
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if(isKnocked)
        {
            return;
        }
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion 
    #region collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);  
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
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
}
