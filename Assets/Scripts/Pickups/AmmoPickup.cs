using UnityEngine;

// Ammo crate inherits from Pickup and refills the truck's gun
public class AmmoPickup : Pickup
{
    [SerializeField] private int ammoAmount = 15;

    // Finds the truck's gun and gives it more ammo
    protected override void Apply(GameObject truck)
    {
        ShooterBullet shooter = truck.GetComponent<ShooterBullet>();
        if (shooter != null)
        {
            shooter.AddAmmo(ammoAmount);
        }
    }
}
