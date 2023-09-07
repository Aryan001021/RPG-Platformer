 using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    CircleCollider2D circleCollider;
    private Player player;
    private bool canRotate=true;
    private bool isReturning=false;
    float FreezeTimeDuration;
    float returnSpeed=12;
    [Header("PierceInfo")]
    [SerializeField] int pierceAmount;
    [Header("BounceInfo")]
    float bounceSpeed;
    bool isBouncing;
    int amountOfBounce;
    List<Transform> enemyTarget=new List<Transform>();
    private int targetIndex;
    [Header("SpinInfo")]
    float maxTravelDistance;
    private float spinDuration;
    float spinTimer;
    bool wasStopped;
    bool isSpinning;
    float hitTimer;
    float hitCoolDown;
    float spinDirection;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);

        }
        SetUpTargetForBounce(collision);
        StuckInto(collision);
    }
    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoDamage(enemy.transform.GetComponent<CharacterStats>());
        enemy.StartCoroutine("FreezeTimeFor", FreezeTimeDuration);
    }
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    public void SetUpSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player=_player;
        rb.gravityScale = _gravityScale;
        rb.velocity = _dir;
        FreezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        if (pierceAmount <= 0) 
        {
            animator.SetBool("Rotation", true);
        }
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
        Invoke("DestroyMe", 7);
    }
    void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void SetUpTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var collider in colliders)
                {
                    if (collider.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(collider.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0&&collision.GetComponent<Enemy>()!=null)
        {
            pierceAmount--;
            return;
        }
        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }
        canRotate = false;
        circleCollider.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (isBouncing && enemyTarget.Count>0)
        {
            return;
        }
        animator.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    public void SetUpBounce(bool _isBouncing,int _amountOfBounce,float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
        bounceSpeed = _bounceSpeed;
    }
    public void SetUpPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    public void SetUpSpin(bool _isSpinning,float _maxTravelDistance,float _spinDuration,float _hitCoolDown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCoolDown = _hitCoolDown;
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }
    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity.normalized;
        }
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }
        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                if (spinTimer < 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }
                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCoolDown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var collider in colliders)
                    {
                        if (collider.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(collider.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                amountOfBounce--;
                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }
}
