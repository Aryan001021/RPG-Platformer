using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    EntityFX FX;

    [Header("Major Stats")]
    public Stats strength;  //1 point  increase damage by 1 and crit power by 1%
    public Stats agility;   // 1 pint increse evasion  by 1% and crit chance by 1%
    public Stats intelligence;  //1 point increase magic damage by 1 and magic resistance by 3
    public Stats vitality;// 1 point  increase hp by 3 or 5

    [Header("Offensive Stats")]
    public Stats damage;
    public Stats critChance;
    public Stats critDamage;

    [Header("Defensive Stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;
    public Stats magicResistance;

    [Header("Magic Stats")]
    public Stats fireDamage;
    public Stats lightningDamage;
    public Stats iceDamage;

    [SerializeField] float ailmentsDuration = 4;
    public bool isIgnited;//does damage over time
    public bool isChill; // reduce armor by  20%
    public bool isShocked;//reduce accuracy by 20%

    float ignitedTimer;
    float chillTimer;
    float shockedTimer;
    float igniteDamageCoolDown=.3f;
    float igniteDamageTimer;
    int igniteDamage;

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
        }
        if(igniteDamageTimer < 0 && isIgnited)
        {
            igniteDamageTimer = igniteDamageCoolDown;
            DecreaseHealthBy(igniteDamage);
            if(currentHealth < 0)
            {
                Die();
            }
        }
        if(chillTimer < 0)
        {
            isChill = false;
        }
        if(shockedTimer < 0)
        {
            isShocked = false;
        }
    }
    public int GetMaxHealth()
    {
        return maxHealth.GetValue()+vitality.GetValue()*5;
    }
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
        DoMagicalDamage(_targetStats);
    }
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage = CalculateTargetMagicResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        if(Mathf.Max(_fireDamage,_iceDamage,_lightningDamage)<=0)
        {
            return;
        }
        bool canApplyIgnite=_fireDamage>_iceDamage&& _fireDamage>_lightningDamage;
        bool canApplyChill= _iceDamage> _fireDamage && _iceDamage>_lightningDamage;
        bool canApplyShock=_lightningDamage>_iceDamage&& _lightningDamage>_fireDamage;

        while(!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if(Random.value<.5&&_fireDamage>0)
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

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }
        _targetStats.ApplyAilments(canApplyIgnite,canApplyChill, canApplyShock);
    }

    private int CalculateTargetMagicResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
        if (isChill || isIgnited || isShocked)
        {
            return;
        }
        if (_ignite)
        {
            isIgnited=_ignite;
            ignitedTimer = ailmentsDuration;
            FX.IgniteFXFor(ailmentsDuration);
        }
        if (_chill) 
        {
            isChill=_chill;
            chillTimer = ailmentsDuration;
            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            FX.ChillFXFor(ailmentsDuration);
        }
        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = ailmentsDuration;
            FX.ShockFXFor(ailmentsDuration);
        }
    }
    public void SetupIgniteDamage(int _damage)
    {
        igniteDamage = _damage;
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        if (currentHealth<=0)
        {
            Die();
        }
       
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
         onHealthChanged?.Invoke();
    }
    protected virtual void Die()
    {

    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (_targetStats.isShocked)
        {
            totalEvasion += 20;
        }
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("attack missed");
            return true;
        }
        return false;
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
}
