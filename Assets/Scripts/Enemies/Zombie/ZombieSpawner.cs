using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombies")]
    public GameObject[] zombiePrefabs;

    [Header("Spawning")]
    public TruckPawn truck;
    public float spawnY = 11f;
    public float minShoulderX = 4.5f;
    public float maxShoulderX = 9f;
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 2.5f;

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
            SpawnZombie();
            spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    private void SpawnZombie()
    {
        float x = Random.Range(minShoulderX, maxShoulderX);
        if (Random.Range(0, 2) == 0)
        {
            x = -x;
        }

        Vector3 spawnPosition = new Vector3(x, spawnY, 0f);
        GameObject prefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
        GameObject newZombie = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);

        newZombie.GetComponent<ScrollWithRoad>().truck = truck;
    }
}