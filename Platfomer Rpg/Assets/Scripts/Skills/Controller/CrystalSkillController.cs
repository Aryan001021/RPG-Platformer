using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    float crystalExitTimer;
    Animator animator;
    CircleCollider2D circleCollider;
    [Header("Explosive Crystal")]
    bool canExplode;
    private bool canGrow;
    float growSpeed=5;
    [Header("Moving Crystal")]
    bool canMoveToEnemy;
    float moveSpeed;
    private void Awake()
    {
        animator = GetComponent<Animator>(); 
        circleCollider = GetComponent<CircleCollider2D>();
    }
    public void SetupCrystal(float _crystalDuration,bool _canExplode,bool _canMoveToEnemy,float _moveSpeed)
    {
        crystalExitTimer = _crystalDuration;
        canExplode = _canExplode;
        canMoveToEnemy = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
    }
    public void Update()
    {
        crystalExitTimer -= Time.deltaTime;
        if (crystalExitTimer < 0)
        {
            FinishCrystal();
        }
        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }
    void AnimatorExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
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

