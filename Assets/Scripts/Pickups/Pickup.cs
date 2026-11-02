using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Apply(other.gameObject);

        if (GameManager.instance != null)
        {
            GameManager.instance.PlaySound(pickupSound);
        }

        Destroy(gameObject);
    }

    protected abstract void Apply(GameObject truck);
}