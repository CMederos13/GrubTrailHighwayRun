using UnityEngine;

// Repair kit inherits from Pickup and heals the truck
public class RepairPickup : Pickup
{
    [SerializeField] private float healAmount = 20f;

    // Finds the truck's Health and heals it
    protected override void Apply(GameObject truck)
    {
        Health health = truck.GetComponent<Health>();
        if (health != null)
        {
            health.Heal(healAmount);
        }
    }
}
