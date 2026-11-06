using UnityEngine;

// What happens when the truck runs out of health: lose a life and repair, or Game Over
public class TruckDeath : Death
{
    // Empty on purpose: the truck has nothing to set up or check every frame
    public override void Start()
    {
    }

    public override void Update()
    {
    }

    public override void Die()
    {
        // Plays the explosion sound
        GameManager.instance.PlaySound(GameManager.instance.explosionSound);

        // Takes away one life; LoseLife tells us if any are left
        bool hasLivesLeft = GameManager.instance.LoseLife();
        if (hasLivesLeft)
        {
            // Still has lives: fully repair the truck and keep driving
            Health health = GetComponent<Health>();
            health.Heal(health.maxHealth);
        }
        else
        {
            // No lives left: destroy the truck so the GameManager shows Game Over
            Destroy(gameObject);
        }
    }
}
