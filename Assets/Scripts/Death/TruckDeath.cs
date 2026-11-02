using UnityEngine;

public class TruckDeath : Death
{
    public override void Start()
    {
    }

    public override void Update()
    {
    }

    public override void Die()
    {
        GameManager.instance.PlaySound(GameManager.instance.explosionSound);

        bool hasLivesLeft = GameManager.instance.LoseLife();
        if (hasLivesLeft)
        {
            Health health = GetComponent<Health>();
            health.Heal(health.maxHealth);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}