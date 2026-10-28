using UnityEngine;

// Handles an enemy dying: splits into smaller pieces, adds score, plays the explosion
public class DeathDestroyManager : Death

{
    [Header("Score")]
    public int scoreValue; // Points the player gets for destroying Obstacles and Enemies
    private Obstacle ObstacleToRemoveOnDeath;

    [Header("Splitting")]
    public GameObject splitPreFab; // smaller asteroid to spawn on death (Leave empty for no split wanted)
    public int splitCount = 2; //Designer can set up how many pieces spawn

    //Called when health reaches 0
    public override void Die()
    {
        // Spawn the smaller pieces FIRST so the obstacle list never hits 0 between frames
        if (splitPreFab != null)
        {
            for (int i = 0; i < splitCount; i++)
            {
                Instantiate(splitPreFab, transform.position, Quaternion.identity, transform.parent);
            }
        }

        if (GameManager.instance != null)
        {
            if (GameManager.instance.obstacleList != null && ObstacleToRemoveOnDeath != null)
            {
                GameManager.instance.obstacleList.Remove(ObstacleToRemoveOnDeath);
            }

            GameManager.instance.score += scoreValue;

            // Play the explosion sound through SFX Player
            GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.deathSound);
        
        }
        
        // Removes the enemy from the scene
        Destroy(gameObject);
    }

    // Finds this enemy's Obstacle so it can be removed from the list on death
    public override void Start()
    {
        ObstacleToRemoveOnDeath = GetComponent<Obstacle>();
    }

    // Required by Death; nothing to do every frame
    public override void Update()
    {
        
    }


}
