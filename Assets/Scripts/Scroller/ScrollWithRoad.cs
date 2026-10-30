using UnityEngine;

// Moves a roadside object down at the road's speed and removes it off screen
public class ScrollWithRoad : MonoBehaviour
{
    public TruckPawn truck;
    public float destroyY = -10f; // How far down it goes before being deleted 

    public void Update()
    {
        // Stops moving once the truck is destroyed 
        if (truck == null)
        {
            return;
        }

        // Slides down at the same speed as the road
        transform.position = transform.position + Vector3.down * truck.currentSpeed * Time.deltaTime;

        // Deletes it once it's below the bottom of the screen
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}