using UnityEngine;

//Shoots bullets from a spawn pont (used by the player's truck)
public class ShooterBullet : Shooter
{
    [Header("Bullet Settings")]
    public Transform bulletSpawnPoint; // Where the bullet spawns (Truck's gun)
    public GameObject bulletPrefab; // the bullet that gets fired 

    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo = 30;

    [Header("Shell Casings")]
    public GameObject shellCasingPrefab;

    // Creates a bullet at the spawn point, facing the same direction as the truck's gun
    public override void Shoot()
    {
        if (currentAmmo <= 0)
        {
            return;
        }

        if (bulletSpawnPoint != null && bulletPrefab != null)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            currentAmmo = currentAmmo - 1;

            if (shellCasingPrefab != null)
            {
                Instantiate(shellCasingPrefab, transform.position, Quaternion.identity);
            }

            GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.fireGunSound);
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
    }

    // Required by shooter; nothing to set up or update every frame
    public override void Start()
    {
        
    }
    public override void Update()
    {
        
    }
}