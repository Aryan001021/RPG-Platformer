using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Check")]
    public Transform attackCheck;
    public float attackCheckRadius;
    public SpriteRenderer spriteRenderer;
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
    public CapsuleCollider2D capsuleCollider { get; private set; }
    protected bool facingRight = true;
    #region Component

    public EntityFX fX { get; private set; }
    public Animator anim { get; private set; }
    public CharacterStats stats { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion
    public System.Action onFlipped;
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer= GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fX = GetComponent<EntityFX>();
        stats = GetComponent<CharacterStats>(); 
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
    protected virtual void Update()
    {

    }
    public virtual void SlowEntityBy(float _slowPercentage,float _slowDuration)
    {

    }
    protected virtual void ReturnToDefaultSpeed()
    {
        anim.speed = 1;
    }
    public virtual void DamageImpact ()
    {
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
        onFlipped?.Invoke();
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
   
    public virtual void Die()
    {

    }
}
