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
        entity = GetComponentInParent<Entity>();
        myStats = GetComponentInParent<CharacterStats>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        entity.onFlipped += FlipUI;//subscribe event of flip on entity
        myStats.onHealthChanged += UpdateHealthUI;//subscribe event of healthchange on characterstats
        UpdateHealthUI();//initialize itself
    }
    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }//flip whenever entity flip
    void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealth();
        slider.value = myStats.currentHealth;
    }
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }//unsubscribe to events
}
