using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    EntityFX FX;

    [Header("Major Stats")]
    public Stats strength;  //1 point  increase damage by 1 and crit power by 1%
    public Stats agility;   // 1 point increse evasion  by 1% and crit chance by 1%
    public Stats intelligence;  //1 point increase magic damage by 1 and magic resistance by 3
    public Stats vitality;// 1 point  increase hp by 3 or 5

    [Header("Offensive Stats")]
    public Stats damage;//the  base damage that hit the opponent
    public Stats critChance;//the percentage of time crit damage will apply
    public Stats critDamage;//the crit damage that mutiplied to the damage when crit chance is in favour

    [Header("Defensive Stats")]
    public Stats maxHealth;//max Hp
    public Stats armor;//added to the hp by armor and such
    public Stats evasion;//chance to evade the attack
    public Stats magicResistance;//defence against magic

    [Header("Magic Stats")]
    public Stats fireDamage;//fire attack damage
    public Stats lightningDamage;//lightning attack damage
    public Stats iceDamage;//freeze attack damage

    [SerializeField] float ailmentsDuration = 4;
    public bool isIgnited;//does damage over time
    public bool isChill; // reduce armor by  20%
    public bool isShocked;//reduce accuracy by 20%

    float ignitedTimer;//timer for how long ignite will be active after applied
    float chillTimer;//timer for how long frost will be active after applied
    float shockedTimer;//timer for how long shock will be active after applied
    float igniteDamageCoolDown=.3f;//cooldown timer for ignite
    float igniteDamageTimer;
    int igniteDamage;
    int shockedDamage;
    [SerializeField] GameObject shockStrikePrefab;//the lighning prefab
    protected bool isDead;

    public int currentHealth;
    public System.Action onHealthChanged;
    protected virtual void Start()
    {
        FX = GetComponent<EntityFX>();
        currentHealth = GetMaxHealth();
        critDamage.SetDefaultValue(150);
    }
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chillTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;
        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }//when ignite timer <0 burn/ignite cancel on self
        ApplyIgniteDamage();
        if (chillTimer < 0)
        {
            isChill = false;
        }
        if (shockedTimer < 0)
        {
            isShocked = false;
        }
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0 && isIgnited)
        {
            igniteDamageTimer = igniteDamageCoolDown;
            DecreaseHealthBy(igniteDamage);
            if (currentHealth < 0&&!isDead)
            {
                Die();
            }
        }
    }//apply ignite damage if entity is burned i.e. ignite is on and the ignitedamagetimer go below 0.here we reset timer 

    public int GetMaxHealth()
    {
        return maxHealth.GetValue()+vitality.GetValue()*5;
    }//return max health on this enitity by getting maxhealth stat value + vitality*5.
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue();

        if (CanCrit())
        {
            totalDamage=CalculateCritDamage(totalDamage);   
            Debug.Log("critDamage="+totalDamage);
        }

        totalDamage = CheckTargetArmer(totalDamage,_targetStats);
        _targetStats.TakeDamage(totalDamage);
    }//used when this entity attact it find this script on enemy entity and decrease the health with by finding its take damage function
    #region Magical Damage
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage = CalculateTargetMagicResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }
        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }//used  when this entity attact it find this script on enemy entity and decrease the health with by finding its take damage function and apply the
     //magic -ve stats like burn ,freeze or shock to other entity

    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {//
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _iceDamage && _lightningDamage > _fireDamage;
     //here we check if we can apply a magic effect with easy logic
     //
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .5 && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5 && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5 && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }
    //if above logic fail then we apply the complex logic to apply magic effect it's an exception handling
        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }//the ignite decrease health perodically so this function
        if (canApplyShock)
        {
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_lightningDamage * .2f));
        }//the shock decrease health perodically so this function
        //also freeze do not effect perodically
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }//where we check which magic effect we can apply
    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChill && !isShocked;
        bool canApplyChill = !isIgnited && !isChill && !isShocked;
        bool canApplyShock = !isIgnited && !isChill;
        if (_ignite&& canApplyIgnite)
        {
            isIgnited=_ignite;
            ignitedTimer = ailmentsDuration;
            FX.IgniteFXFor(ailmentsDuration);
        }//if entity is not ignited already then we apply it otherwise not
        if (_chill&&canApplyChill) 
        {
            isChill=_chill;
            chillTimer = ailmentsDuration;
            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            FX.ChillFXFor(ailmentsDuration);
        }//if entity is not freezed already then we apply it otherwise not
        if (_shock&&canApplyShock)
        {
            if(!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                {
                    return;
                }
                HitNearestTargetWithShockStrike();
            }
        }//if entity is not shocked already then we apply it otherwise not
    }//here we apply effect

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
        {
            return;
        }
        isShocked = _shock;
        shockedTimer = ailmentsDuration;
        FX.ShockFXFor(ailmentsDuration);
    }//shock logic same as fire and freeze in above function

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25f);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {
            if ((hit.GetComponent<Enemy>() != null) && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }
        if (closestEnemy != null)
        {
            GameObject newShockSrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockSrike.GetComponent<ThunderStrikeController>().Setup(shockedDamage, closestEnemy.GetComponent<CharacterStats>());
            Debug.Log("in shock strike");
        }
    }//if we hit an enemy that is already shocked then we can throw lightning on nearby enemy or same enemy

    public void SetupIgniteDamage(int _damage)
    {
        igniteDamage = _damage;
    }
    public void SetupShockDamage(int _damage)
    {
        shockedDamage = _damage;
    }
    #endregion

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        GetComponent<Entity>().DamageImpact();
        if (currentHealth<=0&& !isDead)
        {
            Die();
        }
       
    }//when oppenent entity hit this it try to find this characterstats scipt and call this function to decrease this enitity health
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
         onHealthChanged?.Invoke();
    }//here we decrease health and update health ui by firing event
    protected virtual void Die()
    {
        isDead = true;
    }
    #region Stats Calculation
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (_targetStats.isShocked)
        {
            totalEvasion += 20;
        }
        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }
    private int CalculateTargetMagicResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {

        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }
    private int CheckTargetArmer(int totalDamage,CharacterStats _targetStats)
    {
        if (_targetStats.isChill)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private bool CanCrit()
    {
        int totalCritChance=critChance.GetValue()+agility.GetValue();
        if(Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }
        return false;
    }
    private int CalculateCritDamage(int _damage)
    {
        float totalCritDamage=(critDamage.GetValue()+strength.GetValue())*.01f;
        float critDamageVar = _damage * totalCritDamage;
        return (int) critDamageVar;
    }
    #endregion//in this stats is calculted
}
