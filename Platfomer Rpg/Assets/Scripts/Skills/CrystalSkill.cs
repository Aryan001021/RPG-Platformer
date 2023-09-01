using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] float crystalDuration;
    [SerializeField] GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] bool canMoveToEnemy;
    [SerializeField] float moveSpeed;
    public override void UseSkill()
    {
        base.UseSkill();
        if (currentCrystal == null)
        {
            currentCrystal=Instantiate(crystalPrefab,player.transform.position,Quaternion.identity);
            CrystalSkillController crystalSkillController = currentCrystal.GetComponent<CrystalSkillController>();
            crystalSkillController.SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,moveSpeed);
        }
        else
        {
            Vector2 playerPos= player.transform.position;
            player.transform.position=currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;
            currentCrystal.GetComponent<CrystalSkillController>().FinishCrystal();
        }
    }

}
