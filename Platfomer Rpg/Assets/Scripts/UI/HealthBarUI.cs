using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    Entity entity;
    CharacterStats myStats;
    RectTransform myTransform;
    Slider slider;
    void Start()
    {
        entity=GetComponentInParent<Entity>();
        myStats=GetComponentInParent<CharacterStats>();
        myTransform=GetComponent<RectTransform>();
        slider=GetComponentInChildren<Slider>();
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged+=UpdateHealthUI;
        UpdateHealthUI();
    }
    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }
    void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealth();
        slider.value = myStats.currentHealth;
    }
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
