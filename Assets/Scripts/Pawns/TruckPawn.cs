using UnityEngine;

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

    public override void MoveLeft()
    {
        if (currentLane > 0)
        {
            currentLane = currentLane - 1;
        }
    }

    public override void MoveRight()
    {
        if (currentLane < laneCount - 1)
        {
            currentLane = currentLane + 1;
        }
    }

    public override void SpeedUp()
    {
        currentSpeed = currentSpeed + acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
    }

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

    private float GetLaneX(int lane)
    {
        float firstLaneX = -(laneCount - 1) * laneWidth / 2f;
        return firstLaneX + lane * laneWidth;
    }
}