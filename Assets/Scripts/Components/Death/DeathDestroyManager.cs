using UnityEngine;

// Handles an enemy dying: splits into smaller pieces, adds score, plays the explosion
public class DeathDestroyManager : Death

{
    [Header("Score")]
    public int scoreValue; // Points the player gets for destroying zombies and wrecks
    private Enemy enemyToRemoveOnDeath;

    [Header("Splitting")]
    public GameObject splitPreFab; // smaller piece to spawn on death (Leave empty for no split wanted)
    public int splitCount = 2; //Designer can set up how many pieces spawn

    //Called when health reaches 0
    public override void Die()
    {
        // Spawn the smaller pieces FIRST so the enemy list never hits 0 between frames
        if (splitPreFab != null)
        {
            for (int i = 0; i < splitCount; i++)
            {
                Instantiate(splitPreFab, transform.position, Quaternion.identity, transform.parent);
            }
        }

        if (GameManager.instance != null)
        {
            if (GameManager.instance.enemyList != null && enemyToRemoveOnDeath != null)
            {
                GameManager.instance.enemyList.Remove(enemyToRemoveOnDeath);
            }

            GameManager.instance.score += scoreValue;

            // Play the explosion sound through SFX Player
            GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.explosionSound);
        
        }
        
        // Removes the enemy from the scene
        Destroy(gameObject);
    }

    // Finds this enemy's Enemy component so it can be removed from the list on death
    public override void Start()
    {
        enemyToRemoveOnDeath = GetComponent<Enemy>();
    }

    // Required by Death; nothing to do every frame
    public override void Update()
    {
        
    }


}
