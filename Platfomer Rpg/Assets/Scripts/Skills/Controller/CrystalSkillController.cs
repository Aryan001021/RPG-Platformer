using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    float crystalExitTimer;
    Animator animator;
    CircleCollider2D circleCollider;
    [Header("Explosive Crystal")]
    bool canExplode;
    private bool canGrow;
    float growSpeed = 5;
    [Header("Moving Crystal")]
    bool canMoveToEnemy;
    float moveSpeed;
    Transform closestTarget;
    [SerializeField] LayerMask whatIsEnemy;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed, Transform _closestTarget)
    {
        crystalExitTimer = _crystalDuration;
        canExplode = _canExplode;
        canMoveToEnemy = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }
    public void Update()
    {
        crystalExitTimer -= Time.deltaTime;
        if (crystalExitTimer < 0)
        {
            FinishCrystal();
        }
        if (canMoveToEnemy)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMoveToEnemy = false;
            }
        }
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHoleSkill.GetBlackHoleRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if (colliders.Length > 0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }
    void AnimatorExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().DamageEffect();
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            animator.SetTrigger("Explode");
        }
        else
        {
            canGrow = true;
            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}

