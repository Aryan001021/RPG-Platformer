using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stats damage;
    public Stats maxHealth;
    [SerializeField]int currentHealth;
    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        int totalDamage=damage.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }
    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        if (currentHealth<=0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {

    }
}
