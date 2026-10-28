using UnityEngine;

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
            SpawnWreck();
            spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    private void SpawnWreck()
    {
        int lane = Random.Range(0, truck.laneCount);
        Vector3 spawnPosition = new Vector3(truck.GetLaneX(lane), spawnY, 0f);
        GameObject newWreck = Instantiate(wreckPrefab, spawnPosition, Quaternion.identity, transform);

        SpriteRenderer sr = newWreck.GetComponent<SpriteRenderer>();
        sr.sprite = wreckSprites[Random.Range(0, wreckSprites.Length)];

        BoxCollider2D box = newWreck.AddComponent<BoxCollider2D>();
        box.isTrigger = true;

        newWreck.GetComponent<ScrollWithRoad>().truck = truck;
    }
}