using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ice And Fire", menuName = "Data/item effect/Ice And Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] GameObject iceAndFirePrefab;
    [SerializeField] float newVelocity;
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player= PlayerManager.instance.player;
        bool thirdAttack = player.primaryAttack.comboCounter == 2;
        if(thirdAttack)
        {
            GameObject newIceAndFire=Instantiate(iceAndFirePrefab, _respawnPosition.position,player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D >().velocity = new Vector2(newVelocity*player.facingDirection,0);
            Destroy(newIceAndFire,10);
        }
    }
}
