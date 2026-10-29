using UnityEngine;

// Spawns a random wreck in a random lane every few seconds
public class WreckSpawner : MonoBehaviour
{
    [Header("Wrecks")]
    public GameObject wreckPrefab;
    public Sprite[] wreckSprites; //[] holds a LIST of sprites (Wrecked Sprites)

    [Header("Spawning")]
    public TruckPawn truck;
    public float spawnY = 8f;
    public float minSpawnTime = 1.5f;
    public float maxSpawnTime = 3f;

    private float spawnTimer;

    // Waits the longest spawn time before the first wreck
    public void Start()
    {
        spawnTimer = maxSpawnTime;
    }

    public void Update()
    {
        // Stops spawning once the truck is destroyed
        if (truck == null)
        {
            return;
        }

        // Counts down, then spawns a wreck and picks a new random wait time
        spawnTimer = spawnTimer - Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnWreck();
            spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    private void SpawnWreck()
    {
        // Picks a random lane and places the wreck above the screen
        int lane = Random.Range(0, truck.laneCount);
        Vector3 spawnPosition = new Vector3(truck.GetLaneX(lane), spawnY, 0f);
        GameObject newWreck = Instantiate(wreckPrefab, spawnPosition, Quaternion.identity, transform);

        // Gives the wreck a random wreck picture
        SpriteRenderer sr = newWreck.GetComponent<SpriteRenderer>();
        sr.sprite = wreckSprites[Random.Range(0, wreckSprites.Length)];

        // Adds the collider after the sprite is set so it fits that wreck
        BoxCollider2D box = newWreck.AddComponent<BoxCollider2D>();
        box.isTrigger = true;

        // Tells the wreck which truck's speed to scroll with
        newWreck.GetComponent<ScrollWithRoad>().truck = truck;
    }
}