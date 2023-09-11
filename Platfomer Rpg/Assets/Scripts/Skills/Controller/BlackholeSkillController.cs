using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] GameObject HotKeyPrefab;
    [SerializeField] List<KeyCode> KeyCodeList;
    List<GameObject> createdHotKey = new List<GameObject>();
    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;

    bool canGrow = true;
    bool canShrink;
    bool canCreateHotKeys = true;
    bool CloneAttackReleased;
    bool playerCanDisappear = true;

    int amountOfAttack = 4;
    float cloneAttackCooldown = .3f;
    float cloneAttackTimer;
    float blackHoleTimer;
    List<Transform> target = new List<Transform>();
    public bool playerCanExitState { get; private set; }
    public void SetUpBlackHole(float _maxsize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCoolDown, float _blackHoleDuration)
    {
        maxSize = _maxsize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttack = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCoolDown;
        blackHoleTimer = _blackHoleDuration;
        if (SkillManager.instance.cloneSkill.crystalInsteadOfClone)
        {
            playerCanDisappear = false;
        }
    }//constructor
    private void Update()
    {
        blackHoleTimer -= Time.deltaTime;
        if (blackHoleTimer < 0)
        {
            blackHoleTimer = Mathf.Infinity;
            if (target.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {

                FinishBlackHoleAbility();
            }
        }
        cloneAttackTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
            PlayerManager.instance.player.fX.MakeTransparent(true);
        }
        CloneAttackLogic();
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (target.Count <= 0)
        {
            return;
        }
        CloneAttackReleased = true;
        canCreateHotKeys = false;
        DestroyHotKeys();
        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.fX.MakeTransparent(true);
        }
    }//make player transparent and start spawing gems

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && CloneAttackReleased && amountOfAttack > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, target.Count);
            float xOffset;
            if (Random.Range(0, 100) > 50)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }
            if (SkillManager.instance.cloneSkill.crystalInsteadOfClone)
            {
                SkillManager.instance.crystalSkill.CreateCrystal();
                SkillManager.instance.crystalSkill.CurrentCrystalChooseRandomEnemy();
            }
            else
            {
                SkillManager.instance.cloneSkill.CreateClone(target[randomIndex], new Vector3(xOffset, 0));
            }
            amountOfAttack--;
            if (amountOfAttack <= 0)
            {
                Invoke("FinishBlackHoleAbility", 1f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        CloneAttackReleased = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            CreateHotKey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<Enemy>()?.FreezeTime(false);

    }

    private void DestroyHotKeys()
    {
        if (createdHotKey.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }
    private void CreateHotKey(Collider2D collision)
    {
        if (KeyCodeList.Count <= 0)
        {
            Debug.LogWarning("not enough keycodes");
            return;
        }
        if (!canCreateHotKeys)
        {
            return;
        }
        collision.GetComponent<Enemy>().FreezeTime(true);
        GameObject newHotKey = Instantiate(HotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);
        KeyCode choosenKey = KeyCodeList[Random.Range(0, KeyCodeList.Count)];
        KeyCodeList.Remove(choosenKey);
        BlackHoleHotKeyController newHotKeyScript = newHotKey.GetComponent<BlackHoleHotKeyController>();
        newHotKeyScript.SetUpHotKey(choosenKey, collision.transform, this);
    }
    public void AddEnemyToList(Transform _enemyTransform) => target.Add(_enemyTransform);
}
