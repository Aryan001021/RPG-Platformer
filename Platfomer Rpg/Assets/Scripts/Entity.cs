using System.Collections;
using UnityEngine;
//this script is used by inhertance only it do not attack to a gameobject
public class Entity : MonoBehaviour
{
    [Header("Collision Check")]//object will check to which other object current gameobject are colliding with include ground and enemy check
                               //using physics2D.raycast or physics2D.circlecastall
    public Transform attackCheck;//it's a transform used as centre of a circle if enemy inside it ,it get damage on attack
    public float attackCheckRadius;//raduis of attack circle
    public SpriteRenderer spriteRenderer;
    [SerializeField] protected Transform groundCheck;//a gameobject used as a point through which we start the ground check raycast
    [SerializeField] protected float groundCheckDistance;//the distance of groundcheck raycast
    [SerializeField] protected Transform wallCheck;//a gameobject used as a point through which we start the wall check raycast
    [SerializeField] protected float wallCheckDistance;//distance of wallcheck raycast
    [SerializeField] protected LayerMask whatIsGround;
    
    [Header("KnockBack Info")]//if current gameobject is hitted then it get pushed back
    [SerializeField] protected Vector2 knockBackDir;//the direction of the puchback force
    [SerializeField] protected float knockBackDuration;//how long it will apply
    protected bool isKnocked;

    public int facingDirection { get; private set; } = 1;
    public CapsuleCollider2D capsuleCollider { get; private set; }//object bound
    protected bool facingRight = true;
    #region Component

    public EntityFX fX { get; private set; }//it contain effects that can be apply on an entity
    public Animator anim { get; private set; }
    public CharacterStats stats { get; private set; }//it contain stats like health intelligence attack and stuff.
    public Rigidbody2D rb { get; private set; }
    #endregion
    public System.Action onFlipped;//event
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fX = GetComponent<EntityFX>();
        stats = GetComponent<CharacterStats>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
    protected virtual void Update()
    {

    }
    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }//it slows the entity by a number when freeze attack is applied on it, it default implementation rest is in its child script.
    protected virtual void ReturnToDefaultSpeed()
    {
        anim.speed = 1;
    }//when freeze is lifted this is called to make player fast again , it default implementation rest is in its child script.
    public virtual void DamageImpact()
    {
        fX.StartCoroutine("FlashFX");
        StartCoroutine(HitKnockBack());
    }//this make object glow when it hit and knockback the object
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
        if (isKnocked)
        {
            return;
        }
        rb.velocity = Vector2.zero;
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
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
    }//used the above flip function according to input and used by SetVelocity()
    #endregion

    public virtual void Die()
    {

    }
}
