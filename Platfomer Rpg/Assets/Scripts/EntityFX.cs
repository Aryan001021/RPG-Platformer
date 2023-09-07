using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [SerializeField] Material hitMaterial;
    private Material originalMaterial;
    SpriteRenderer spriteRenderer;
    [Header("Ailments Colors")]
    [SerializeField] Color chillColor;
    [SerializeField] Color[] igniteColor;
    [SerializeField] Color[] shockColor;

    void Start()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
        originalMaterial=spriteRenderer.material;
    }
    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            spriteRenderer.color = Color.clear;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMaterial;
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = currentColor;
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
    private void CancelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
    public void ChillFXFor(float _seconds)
    {
        InvokeRepeating("ChillColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void IgniteFXFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ShockFXFor(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void IgniteColorFX()
    {
        if(spriteRenderer.color != igniteColor[0])
        {
            spriteRenderer.color = igniteColor[0];
        }
        else
        {
            spriteRenderer.color = igniteColor[1];
        }
    }
    private void ChillColorFX()
    {
        spriteRenderer.color = chillColor;
    }
    private void ShockColorFX()
    {
        if (spriteRenderer.color != shockColor[0])
        {
            spriteRenderer.color = shockColor[0];
        }
        else
        {
            spriteRenderer.color = shockColor[1];
        }
    }
}
