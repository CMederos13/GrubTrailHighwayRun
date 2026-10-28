using UnityEngine;

// Pushes the bullet forward when it's fired 
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float fireForce; // how hard the bullet is pushed

    public float lifetime; // How long the bullet lasts before being removed

    private Rigidbody2D rb;

    private Transform tf;

    // Pushes the bullet forward the way the ship's facing, and sets it up to be destroyed
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        tf = GetComponent<Transform>();

        if (rb != null && tf != null)
        {
            rb.AddForce(tf.up * fireForce);
        }
        // Removes the bullet after "lifetime" seconds so missed shots don't pile up 
        Destroy(gameObject, lifetime);
    
        
    }
    
}
