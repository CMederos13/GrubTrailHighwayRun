using UnityEngine;

public class ScrollWithRoad : MonoBehaviour
{
    public TruckPawn truck;
    public float destroyY = -10f;

    public void Update()
    {
        if (truck == null)
        {
            return;
        }

        transform.position = transform.position + Vector3.down * truck.currentSpeed * Time.deltaTime;

        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}