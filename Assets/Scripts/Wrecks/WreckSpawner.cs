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
    public float clearCheckHeight = 3f;

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
            spawnTimer = Random.Range(minSpawnTime, maxSpawnTime) / GameManager.instance.GetDifficulty();
        }
    }

    private void SpawnWreck()
    {
        // Picks a random lane, then tries the other lanes until it finds an empty one
        int startLane = Random.Range(0, truck.laneCount);
        for (int i = 0; i < truck.laneCount; i++)
        {
            int lane = (startLane + i) % truck.laneCount;
            Vector3 spawnPosition = new Vector3(truck.GetLaneX(lane), spawnY, 0f);

            if (IsSpotClear(spawnPosition))
            {
                PlaceWreck(spawnPosition);
                return;
            }
        }
        // Every lane is blocked, so this wreck is skipped
    }

    // Places the wreck above the screen in the empty lane
    private void PlaceWreck(Vector3 spawnPosition)
    {
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

    // Checks a box the size of a lane at the spawn point; true if nothing (wreck, obstacle, pickup or zombie) is there
    private bool IsSpotClear(Vector3 position)
    {
        Physics2D.SyncTransforms();
        Vector2 checkSize = new Vector2(truck.laneWidth * 0.8f, clearCheckHeight);
        Collider2D hit = Physics2D.OverlapBox(position, checkSize, 0f);
        return hit == null;
    }
}
