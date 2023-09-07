using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("CloneInfo")]
    [SerializeField] GameObject clone; 
    [SerializeField] float cloneDuration;
    [Space]
    [SerializeField] bool canAttack;
    [SerializeField] bool createCloneOnDashStart;
    [SerializeField] bool createCloneOnDashOver;
    [SerializeField] bool canCreateCloneOnCounterAttack;
    [Header("Clone Can Duplicate")]
    [SerializeField] bool canDuplicateClone;
    [SerializeField] float chanceToDuplicate;
    [Header("Crystal Instead Of Clone")]
    public bool crystalInsteadOfBone;
    public void CreateClone(Transform _transform,Vector3 _offset)
    {
        if(crystalInsteadOfBone)
        {
            SkillManager.instance.crystalSkill.CreateCrystal();
            return;
        }
        GameObject newClone=Instantiate(clone);
        newClone.GetComponent<CloneSkillController>().
            SetupClone(_transform,cloneDuration, canAttack,_offset,FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player);
    }
    public void CreateCloneOnDashStart()
    {
        if(createCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }
    public void CreateCloneOnDashEnd()
    {
        if(createCloneOnDashOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if(canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDirection, 0, 0)));
        }
    }
    IEnumerator CreateCloneWithDelay(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
            CreateClone(_transform,_offset);
    }
}
