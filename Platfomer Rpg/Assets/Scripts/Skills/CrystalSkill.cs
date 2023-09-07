using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] float crystalDuration;
    [SerializeField] GameObject crystalPrefab;
    private GameObject currentCrystal;
    [Header("Crystal Mirage")]
    [SerializeField] bool cloneInsteadOfCrystal;
    [Header("Explosive Crystal")]
    [SerializeField] bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] bool canMoveToEnemy;
    [SerializeField] float moveSpeed;
    [Header("Multi Stacking Crystal")]
    [SerializeField] bool canUseMultiStacks;
    [SerializeField]int amountOfStacks;
    [SerializeField] float useTimeWindow;
    [SerializeField] float multiStackCooldown;
    [SerializeField]List<GameObject>crystalLeft=new List<GameObject>();
    public override void UseSkill()
    {
        base.UseSkill();
        if(CanUseMultiCrystal())
        {
            return;
        }
        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
            {
                return;
            }
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;
            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.cloneSkill.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<CrystalSkillController>().FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillController crystalSkillController = currentCrystal.GetComponent<CrystalSkillController>();
        crystalSkillController.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }
    public void CurrentCrystalChooseRandomEnemy()
    {
        currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if(crystalLeft.Count>0)
            {
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow); 
                }
                coolDown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<CrystalSkillController>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);
                if(crystalLeft.Count <= 0)
                {
                    coolDown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }
        return false;
    }
    private void RefillCrystal()
    {
        int amountToAdd=amountOfStacks-crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }
    private void ResetAbility()
    {
        if (coolDownTimer > 0)
        {
            return;
        }
        coolDownTimer = multiStackCooldown;
        RefillCrystal();
    }

}
