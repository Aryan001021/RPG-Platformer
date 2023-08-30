using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotKeyController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    KeyCode myHotKey;
    TextMeshProUGUI myText;
    Transform myEnemy;
    BlackholeSkillController blackHole;
    public void SetUpHotKey(KeyCode _hotKey,Transform _enemyTransform,BlackholeSkillController _blackHoleController)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myHotKey = _hotKey;
        myText.text=myHotKey.ToString();
        myEnemy = _enemyTransform;
        blackHole = _blackHoleController;
    }
    private void Update()
    {
        if(Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }
}
