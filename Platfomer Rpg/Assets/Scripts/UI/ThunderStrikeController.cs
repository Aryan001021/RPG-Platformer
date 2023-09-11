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
        anim = GetComponentInChildren<Animator>();
    }
    public void Setup(int _damage, CharacterStats _targetStats)
    {
        targetStats = _targetStats;
        damage = _damage;
    }//constructor
    void Update()
    {
        if (!targetStats)
        {
            return;
        }//if there is no target return
        if (triggered)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);//move towards target
        transform.right = transform.position - targetStats.transform.position;//set transforms right
        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localPosition = new Vector3(0, .5f);
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true;
            anim.SetTrigger("Hit");
        }
    }//getting very close stop  moving and rotating then we make it bigger then change animation to hit and call destroy itself 
    void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);//apply shoch to target
        targetStats.TakeDamage(damage);//do damage to target
        Destroy(gameObject, .4f);//destroyed

    }
}
