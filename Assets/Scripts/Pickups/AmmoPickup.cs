using UnityEngine;

public class AmmoPickup : Pickup
{
    [SerializeField] private int ammoAmount = 15;

    protected override void Apply(GameObject truck)
    {
        ShooterBullet shooter = truck.GetComponent<ShooterBullet>();
        if (shooter != null)
        {
            shooter.AddAmmo(ammoAmount);
        }
    }
}