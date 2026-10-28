using UnityEngine;

// Damages anything with Health it touches, then destroys itself (used by bullets and enemies) 
public class Damage : MonoBehaviour
{

    [Header("Damage")]
    public float damageAmount; // How much health this takes away

    public bool isInstaKill; // If checked , kills instantly no matter the health

    private Health OtherHealthComponent;

    // When this touches something with health; deal damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OtherHealthComponent = collision.GetComponent<Health>();

        if (OtherHealthComponent != null)
        {
            //Insta-kill deals the target's full max health
            if (isInstaKill)
            {
                OtherHealthComponent.TakeDamage(OtherHealthComponent.maxHealth);
            }
            else
            {
                OtherHealthComponent.TakeDamage(damageAmount);
            }
            // The bullet or enemy that hit is destroyed on contact 
            Destroy(gameObject);

        }

    }
}