using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField] float colorLosingSpeed;
    [SerializeField] GameObject attackCheck;
    [SerializeField] float attackCheckRadius=.8f;
    private float cloneTimer;
    Transform closestEnemy;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer-=Time.deltaTime;
        if(cloneTimer < 0 )
        {
            spriteRenderer.color= new Color(1,1,1,spriteRenderer.color.a-Time.deltaTime*colorLosingSpeed);
            if (spriteRenderer.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetupClone(Transform _transform,float _cloneDuration,bool _canAttack,Vector3 _offset)
    {
        transform.position = _transform.position+_offset;
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
                hit.GetComponent<Enemy>().Damage();
            }
        }

    }
    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25f);
        float closestDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {
            if((hit.GetComponent<Enemy>() != null))
            {
                float distanceToEnemy=Vector2.Distance(transform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance=distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
