using UnityEngine;

public class RepairPickup : Pickup
{
    [SerializeField] private float healAmount = 20f;

    protected override void Apply(GameObject truck)
    {
        Health health = truck.GetComponent<Health>();
        if (health != null)
        {
            health.Heal(healAmount);
        }
    }
}