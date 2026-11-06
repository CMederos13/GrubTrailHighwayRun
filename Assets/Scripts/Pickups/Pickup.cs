using UnityEngine;

// Parent class for every pickup (abstraction): handles the collecting, the child classes decide what it does
public abstract class Pickup : MonoBehaviour
{
    // Private so other scripts can't change it, but still shows in the Inspector (encapsulation)
    [SerializeField] private AudioClip pickupSound;

    // Runs when something drives through the pickup's trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignores anything that isn't the player's truck (zombies, bullets)
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Each child class does its own thing here (polymorphism)
        Apply(other.gameObject);

        // Plays the pickup sound
        if (GameManager.instance != null)
        {
            GameManager.instance.PlaySound(pickupSound);
        }

        // Removes the pickup from the road
        Destroy(gameObject);
    }

    // Every child class must write its own Apply (abstract = no code here)
    protected abstract void Apply(GameObject truck);
}
