using UnityEngine;

// Base class for anything that can shoot (the player ship uses ShooterBullet)
public abstract class Shooter : MonoBehaviour
{
    // Each shooter sets itself up
    public abstract void Start();

    public abstract void Update();

    // Fires one shot; each shooter decides what it fires
    public abstract void Shoot();
}