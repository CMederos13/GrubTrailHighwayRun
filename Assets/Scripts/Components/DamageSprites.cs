using UnityEngine;

public class DamageSprites : MonoBehaviour
{
    public Sprite[] damageSprites;

    private Health health;
    private SpriteRenderer sr;

    public void Start()
    {
        health = GetComponent<Health>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        float healthPercent = health.currentHealth / health.maxHealth;
        int index = damageSprites.Length - Mathf.CeilToInt(healthPercent * damageSprites.Length);
        index = Mathf.Clamp(index, 0, damageSprites.Length - 1);
        sr.sprite = damageSprites[index];
    }
}