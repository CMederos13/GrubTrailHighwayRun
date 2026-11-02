using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Pickups")]
    public GameObject[] pickupPrefabs;

    [Header("Spawning")]
    public TruckPawn truck;
    public float spawnY = 11f;
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 6f;

    private float spawnTimer;

    public void Start()
    {
        spawnTimer = maxSpawnTime;
    }

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
        int lane = Random.Range(0, truck.laneCount);
        Vector3 spawnPosition = new Vector3(truck.GetLaneX(lane), spawnY, 0f);
        GameObject prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];
        GameObject newPickup = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);

        newPickup.GetComponent<ScrollWithRoad>().truck = truck;
    }
}