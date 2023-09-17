using System;
using UnityEngine;
public class EnemyStats : CharacterStats
{
    Enemy enemy;
    [Header("level Details")]
    [SerializeField] int level=1;
    [Range(0f, 1f)]
    [SerializeField] float percentagemodifier=.4f;
    protected override void Start()
    {
        ApplyLevelModifiers();
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);


        Modify(damage);
        Modify(critChance);
        Modify(critDamage);


        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);


        Modify(fireDamage);
        Modify(lightningDamage);
        Modify(iceDamage);
    }

    void Modify(Stats _stats)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stats.GetValue() * percentagemodifier;
            _stats.AddModifier(Mathf.RoundToInt( modifier));
        }
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }//this function is called when an opponent hit player it scans it and use this function to do damage to player

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
