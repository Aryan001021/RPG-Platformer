using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Thunder Strike", menuName = "Data/item effect/thunder strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject thunderStrike= Instantiate(thunderStrikePrefab,_enemyPosition.position,Quaternion.identity); 
        Destroy(thunderStrike,1f);
    }
}
