using UnityEngine;

// The player's military truck: changes lanes, speeds up and slows down, and shoots
public class TruckPawn : Pawn
{
    [Header("Components")]
    public Transform tf;
    private Shooter sh;

    [Header("Lanes")]
    public int laneCount = 4;
    public float laneWidth = 1.8f;
    public float laneChangeSpeed = 10f;
    private int currentLane = 1;

    [Header("Speed")]
    public float minSpeed = 4f;
    public float maxSpeed = 12f;
    public float acceleration = 6f;
    public float currentSpeed = 6f;

    public override void Start()
    {
        tf = GetComponent<Transform>();
        sh = GetComponent<Shooter>();
    }

    public override void Update()
    {
        //slides the truck toward the center of its current lane 
        float targetX = GetLaneX(currentLane);
        Vector3 targetPosition = new Vector3(targetX, tf.position.y, tf.position.z);
        tf.position = Vector3.MoveTowards(tf.position, targetPosition, laneChangeSpeed * Time.deltaTime);
    }

    //Moves one lane left unless already in the leftmost lane 
    public override void MoveLeft()
    {
        if (currentLane > 0)
        {
            currentLane = currentLane - 1;
        }
    }

    // Moves one lane right unless already in the rightmost lane
    public override void MoveRight()
    {
        if (currentLane < laneCount - 1)
        {
            currentLane = currentLane + 1;
        }
    }

    // Speeds up over time, kept between min and max speed
    public override void SpeedUp()
    {
        currentSpeed = currentSpeed + acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
    }

    // Slows down over time, kept between min and max speed
    public override void SlowDown()
    {
        currentSpeed = currentSpeed - acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
    }

    public override void Shoot()
    {
        // telling our shooter component to shoot
        if (sh != null)
        {
            sh.Shoot();
        }
    }

    // Converts a lane number (0-3) into an x position, centered on the road
    public float GetLaneX(int lane)
    {
        float firstLaneX = -(laneCount - 1) * laneWidth / 2f;
        return firstLaneX + lane * laneWidth;
    }
}