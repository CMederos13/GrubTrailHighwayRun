using UnityEngine;

// Spawns a random pickup (food, ammo or repair) in an empty lane every few seconds
public class PickupSpawner : MonoBehaviour
{
    [Header("Pickups")]
    // Adding a prefab more than once makes it come up more often (ammo is in here twice)
    public GameObject[] pickupPrefabs;

    [Header("Spawning")]
    public TruckPawn truck;
    public float spawnY = 11f;
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 6f;
    // How tall the "is this spot empty" box is
    public float clearCheckHeight = 3f;

    private float spawnTimer;

    // Waits the longest spawn time before the first pickup
    public void Start()
    {
        spawnTimer = maxSpawnTime;
    }

    // Counts down, then spawns a pickup and picks a new random wait time
    public void Update()
    {
        if (truck == null)
        {
            return;
        }

        spawnTimer = spawnTimer - Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnPickup();
            spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    private void SpawnPickup()
    {
        // Starts at a random lane, then tries the other lanes until it finds an empty one
        int startLane = Random.Range(0, truck.laneCount);
        for (int i = 0; i < truck.laneCount; i++)
        {
            int lane = (startLane + i) % truck.laneCount;
            Vector3 spawnPosition = new Vector3(truck.GetLaneX(lane), spawnY, 0f);

            if (IsSpotClear(spawnPosition))
            {
                GameObject prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];
                GameObject newPickup = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
                newPickup.GetComponent<ScrollWithRoad>().truck = truck;
                return;
            }
        }
        // Every lane is blocked, so this pickup is skipped
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