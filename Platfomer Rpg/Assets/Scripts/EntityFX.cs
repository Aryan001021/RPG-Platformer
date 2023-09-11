using System.Collections;
using UnityEngine;
//this script contains effect the connect gameobject can do
public class EntityFX : MonoBehaviour
{
    [SerializeField] Material hitMaterial;//material when object it hitted
    private Material originalMaterial;
    SpriteRenderer spriteRenderer;
    [Header("Ailments Colors")]//used to apply magic effect
    [SerializeField] Color chillColor;//freeze effect blue color
    [SerializeField] Color[] igniteColor;//fire effect dark and light red
    [SerializeField] Color[] shockColor;//lightning  dark and light yellow

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
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
    }//make player transparent according to input used by player for black hole skill to vanish when throwing crystals

    IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMaterial;
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMaterial;
    }//when object hit it splashes
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
    }//make object red and normal when it get hit
    private void CancelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }//when magic effect is wore off it is called
    public void ChillFXFor(float _seconds)
    {
        InvokeRepeating("ChillColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }//when object is hit by freeze magic
    public void IgniteFXFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }//when object is hit by fire magic
    public void ShockFXFor(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }//when object is hit by shock magic
    private void IgniteColorFX()
    {
        if (spriteRenderer.color != igniteColor[0])
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
