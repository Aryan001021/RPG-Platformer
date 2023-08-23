using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [SerializeField] Material hitMaterial;
    private Material originalMaterial;
    SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
        originalMaterial=spriteRenderer.material;
    }

    
    IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = originalMaterial; 
    }
    private void RedColorBLink()
    {
        if (spriteRenderer.color != Color.white)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }
    private void CancelRedBlink()
    {
        CancelInvoke("RedColorBLink");
        spriteRenderer.color = Color.white;
    }
}
