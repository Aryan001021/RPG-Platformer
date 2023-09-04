using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField] float colorLosingSpeed;
    [SerializeField] GameObject attackCheck;
    [SerializeField] float attackCheckRadius = .8f;
    private float cloneTimer;
    Transform closestEnemy;
    bool canDuplicateClone;
    int facingDir = 1;
    float chanceToDuplicate;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - Time.deltaTime * colorLosingSpeed);
            if (spriteRenderer.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetupClone(Transform _transform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _chanceToDuplicate)
    {
        transform.position = _transform.position + _offset;
        if (_canAttack)
        {
            if (animator == null)
            {
                Debug.Log("Null");
            }
            else
            {
                animator.SetInteger("AttackNumber", Random.Range(1, 3));
            }
        }
        cloneTimer = _cloneDuration;
        closestEnemy = _closestEnemy;
        chanceToDuplicate = _chanceToDuplicate;
        canDuplicateClone = _canDuplicateClone;
        FaceClosestTarget();
    }
    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }
    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.transform.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().DamageEffect();
                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.cloneSkill.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0, 0));
                    }
                }
            }
        }

    }
    private void FaceClosestTarget()
    {

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
