using UnityEngine;

public class ZombieDeath : Death
{
    public int scoreValue = 10;
    public AudioClip deathSound;

    public override void Start()
    {
    }

    public override void Update()
    {
    }

    public override void Die()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.score = GameManager.instance.score + scoreValue;
        }

        if (deathSound != null)
        {
            Vector3 soundPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            AudioSource.PlayClipAtPoint(deathSound, soundPosition);
        }

        Destroy(gameObject);
    }
}