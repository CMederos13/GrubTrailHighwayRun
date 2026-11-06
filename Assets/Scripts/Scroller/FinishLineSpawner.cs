using UnityEngine;

// Drops the finish line onto the road once, timed so it crosses the truck right when the player wins
public class FinishLineSpawner : MonoBehaviour
{
    public GameObject finishLinePrefab;
    public TruckPawn truck;
    public RoadScroller road;
    public float spawnY = 11f;   // spawns above the top of the screen

    // Makes sure only one finish line is ever made
    private bool hasSpawned = false;

    public void Update()
    {
        // Stops checking once the line is made, or if the truck or road is missing
        if (hasSpawned || truck == null || road == null)
        {
            return;
        }

        // How far is left until the finish distance
        float distanceLeft = GameManager.instance.finishDistance - road.distanceTraveled;

        // When the distance left equals the gap between the spawn point and the truck, spawn it
        // so it scrolls down and reaches the truck exactly at the finish
        if (distanceLeft <= spawnY - truck.transform.position.y)
        {
            Vector3 spawnPosition = new Vector3(0f, spawnY, 0f);
            GameObject finishLine = Instantiate(finishLinePrefab, spawnPosition, Quaternion.identity, transform);

            // Tells the finish line which truck's speed to scroll with
            finishLine.GetComponent<ScrollWithRoad>().truck = truck;
            hasSpawned = true;
        }
    }
}
