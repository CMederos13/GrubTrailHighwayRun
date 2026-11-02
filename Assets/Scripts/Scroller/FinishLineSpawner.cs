using UnityEngine;

public class FinishLineSpawner : MonoBehaviour
{
    public GameObject finishLinePrefab;
    public TruckPawn truck;
    public RoadScroller road;
    public float spawnY = 11f;

    private bool hasSpawned = false;

    public void Update()
    {
        if (hasSpawned || truck == null || road == null)
        {
            return;
        }

        float distanceLeft = GameManager.instance.finishDistance - road.distanceTraveled;
        if (distanceLeft <= spawnY - truck.transform.position.y)
        {
            Vector3 spawnPosition = new Vector3(0f, spawnY, 0f);
            GameObject finishLine = Instantiate(finishLinePrefab, spawnPosition, Quaternion.identity, transform);
            finishLine.GetComponent<ScrollWithRoad>().truck = truck;
            hasSpawned = true;
        }
    }
}
