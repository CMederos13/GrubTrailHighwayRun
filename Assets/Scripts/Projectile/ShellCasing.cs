using UnityEngine;

public class ShellCasing : MonoBehaviour
{
    public float ejectForce = 3f;
    public float spinSpeed = 720f;
    public float lifetime = 1.5f;

    private Rigidbody2D rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float side = 1f;
        if (Random.Range(0, 2) == 0)
        {
            side = -1f;
        }

        Vector2 ejectDirection = new Vector2(side, 1f);
        rb.AddForce(ejectDirection * ejectForce, ForceMode2D.Impulse);
        rb.angularVelocity = Random.Range(-spinSpeed, spinSpeed);

        Destroy(gameObject, lifetime);
    }
}