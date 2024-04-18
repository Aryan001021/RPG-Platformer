using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    [SerializeField] LayerMask winPortal;
    [SerializeField] SpriteRenderer playerSprite;
    public float fadeSpeed = 0.1f;
    [SerializeField]GameObject ReloadCanvas;

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, winPortal))
        {
            
            PlayerManager.instance.player.SetZeroVelocity();
            PlayerManager.instance.player.stateMachine.ChangeState(PlayerManager.instance.player.blankState);
            PlayerManager.instance.player.SetZeroVelocity();
            StartCoroutine(MyCoroutine());
        }
    }

    private IEnumerator MyCoroutine()
    {
        Debug.Log("Here in routine");
        while (playerSprite.color.a > 0)
        {
            yield return new WaitForSeconds(0.1f);
            Color spriteColor = playerSprite.color;
            spriteColor.a -= fadeSpeed * Time.deltaTime;
            playerSprite.color = spriteColor;
        }

        Color spriteColor1 = playerSprite.color;
        // Check if the alpha value reaches or goes below 0
        if (spriteColor1.a <= 0f)
        {
            // Perform any actions when the sprite fades out completely
            gameObject.SetActive(false); // Disable the GameObject or perform other actions
        }
        
        ReloadCanvas.SetActive(true);
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
