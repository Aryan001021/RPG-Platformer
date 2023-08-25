using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill info")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Vector2 launchForce;
    [SerializeField] float swordGavity ;
    Vector2 finalDir;
    [Header("AimDots")]
    [SerializeField] int noOfDots;
    [SerializeField] float spaceBetweenDots;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] GameObject dotsParent;
    GameObject[] dots;
    protected override  void Start()
    {
        base.Start();
        GenerateDots();
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position=DotsPosition(i*spaceBetweenDots);
            }
        }
    }
    public void CreateSword()
    {
        GameObject sword=Instantiate(swordPrefab,player.transform.position,transform.rotation);
        SwordSkillController swordSkillController=sword.GetComponent<SwordSkillController>();
        swordSkillController.SetUpSword(finalDir,swordGavity);
        DotsActive(false);
    }
    public Vector2 AimDirection()
    {
        Vector2 playerPos=player.transform.position;
        Vector2 mousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction=mousePos - playerPos;
        return direction;
    }
    public void DotsActive(bool _isActive)
    {
        for(int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GenerateDots()
    {
        dots=new GameObject[noOfDots];
        for(int i=0;i<noOfDots;i++)
        {
            dots[i]=Instantiate(dotPrefab,player.transform.position,Quaternion.identity,dotsParent.transform);
            dots[i].SetActive(false);
        }
    }
    Vector2 DotsPosition(float _time)
    {
        Vector2 position=(Vector2)player.transform.position+
            new Vector2(AimDirection().normalized.x*launchForce.x,
            AimDirection().normalized.y*launchForce.y)*_time+.5f*(Physics2D.gravity*swordGavity)*(_time*_time);//used Vector distance formulae 
        return position;
    }
}
