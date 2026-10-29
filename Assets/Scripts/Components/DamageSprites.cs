using UnityEngine;

// Swaps the truck's sprite to a more damaged one as its health drops
public class DamageSprites : MonoBehaviour
{
    public Sprite[] damageSprites; // MilitaryTruck1 (new) to MilitaryTruck5 (wrecked)

    private Health health;
    private SpriteRenderer sr;

    public void Start()
    {
        health = GetComponent<Health>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        // Health as a percent: 1 = full, 0 = dead
        float healthPercent = health.currentHealth / health.maxHealth;
        
        // Turns the health percent into which picture to show (0 = new, 4 = wrecked)
        int index = damageSprites.Length - Mathf.CeilToInt(healthPercent * damageSprites.Length);
        
        // Keeps the number inside the list so it never goes out of range
        index = Mathf.Clamp(index, 0, damageSprites.Length - 1);
        sr.sprite = damageSprites[index];
    }
}