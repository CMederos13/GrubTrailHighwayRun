using UnityEngine;

// Base class for anything a controller can move (Player Ship, UFO)
public abstract class Pawn : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public abstract void Start();

    // Update is called once per frame
    public abstract void Update();

    public abstract void MoveLeft();

    public abstract void MoveRight();

    public abstract void SpeedUp();

    public abstract void SlowDown();

    //Fires the pawn's weapon 
    public abstract void Shoot();

}