 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    CircleCollider2D circleCollider;
    private Player player;

    //[Header("Aim dot")]
    //[SerializeField] int noOfDots;
    //[SerializeField] float spaceBetweenDot;
    //[SerializeField] GameObject dotPrefab;
    //[SerializeField] GameObject dotsParent;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    public void SetUpSword(Vector2 _dir, float _gravityScale)
    {
         rb.gravityScale = _gravityScale;
         rb.velocity = _dir;
    }
}
