using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : Skill
{
    [SerializeField] int amountOfAttacks;
    [SerializeField] float cloneCoolDown;
    [SerializeField] float blackHoleDuration;
    [Space]
    [SerializeField] GameObject blackHolePrefab;
    [SerializeField] float maxSize;
    [SerializeField] float growSpeed;
    [SerializeField] float shrinkSpeed;
    BlackholeSkillController currentBlackHole;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole=Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);
        currentBlackHole = newBlackHole.GetComponent<BlackholeSkillController>();
        currentBlackHole.SetUpBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCoolDown,blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    public bool SkillCompleted()
    {
        if (!currentBlackHole)
        {
            return false; 
        }
        if (currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }
        else
        {
            return false;
        }
    }
    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
}
