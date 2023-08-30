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
    public void CreateClone(Transform _transform,Vector3 _offset)
    {
        GameObject newClone=Instantiate(clone);
        newClone.GetComponent<CloneSkillController>().SetupClone(_transform,cloneDuration, canAttack,_offset);
    }
}
