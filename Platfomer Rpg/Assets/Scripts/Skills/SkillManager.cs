using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public DashSkill dashSkill { get; private set; }
    public CloneSkill cloneSkill { get; private set; }
    public SwordSkill swordSkill { get; private set; }
    public BlackHoleSkill blackHoleSkill { get; private set; }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }
    private void Start()
    {
        dashSkill = GetComponent<DashSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        swordSkill = GetComponent<SwordSkill>();
        blackHoleSkill = GetComponent<BlackHoleSkill>();
    }
}
