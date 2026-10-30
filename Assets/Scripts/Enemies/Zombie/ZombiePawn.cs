using UnityEngine;

public class ZombiePawn : Pawn
{
    [Header("Movement")]
    public float walkSpeed = 2.5f;

    private Transform tf;

    public override void Start()
    {
        tf = GetComponent<Transform>();
    }

    public override void Update()
    {

    }

    public void MoveToward(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - tf.position;
        direction.z = 0f;
        direction = direction.normalized;
        tf.position = tf.position + direction * walkSpeed * Time.deltaTime;
    }

    public override void MoveLeft()
    {

    }

    public override void MoveRight()
    {

    }

    public override void SpeedUp()
    {

    }

    public override void SlowDown()
    {

    }

    public override void Shoot()
    {

    }
}