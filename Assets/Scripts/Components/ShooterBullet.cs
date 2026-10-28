using UnityEngine;

//Shoots bullets from a spawn pont (used by the player's ship)
public class ShooterBullet : Shooter
{
    [Header("Bullet Settings")]
    public Transform bulletSpawnPoint; // Where the bullet spawns (Ship's nose)
    public GameObject bulletPrefab; // the bullet that gets fired 

    // Creates a bullet at the spawn point, facing the same direction as the nose of the ship
    public override void Shoot()
    {
        if (bulletSpawnPoint != null && bulletPrefab != null)
        {
            
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            // Plays the gun sound through the GameManager's SFX player

            GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.fireGunSound);
        }
    }
    
    // Required by shooter; nothing to set up or update every frame
    public override void Start()
    {
        
    }
    public override void Update()
    {
        
    }
}