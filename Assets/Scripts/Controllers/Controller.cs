using UnityEngine;

// Base class for anything that controls a pawn (Player or AI)
public abstract class Controller : MonoBehaviour
{
    public Pawn pawn; // the pawn this controller is giving orders to 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public abstract void Start();


    // Update is called once per frame
    public abstract void Update();

    public abstract void MakeDecisions(); // each controller decides what its pawn should do

}