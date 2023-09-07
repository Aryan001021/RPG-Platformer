using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    CharacterStats targetStats;
    [SerializeField] float speed;
    Animator anim;
    int damage;
    bool triggered;
    void Start()
    {
        anim=GetComponentInChildren<Animator>();
    }
    public void Setup(int _damage, CharacterStats _targetStats)
    {
        targetStats=_targetStats;
        damage = _damage;
    }
    // Update is called once per frame
    void Update()
    {
        if (!targetStats)
        {
            return;
        }
        if (triggered)
        {
            return;
        }
        transform.position=Vector2.MoveTowards(transform.position,targetStats.transform.position,speed*Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;
        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localPosition = new Vector3(0, .5f);
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            Invoke("DamageAndSelfDestroy",.2f );
            triggered = true;
            anim.SetTrigger("Hit");
        }
    }
    void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
            targetStats.TakeDamage(damage);
            Destroy(gameObject, .4f);

    }
}
