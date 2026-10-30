using UnityEngine;

// Reads the player's key presses and tells the Truck (pawn) what to do
public class PlayerController : Controller
{
    [Header("Steering")]
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveLeftAltKey = KeyCode.LeftArrow;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode moveRightAltKey = KeyCode.RightArrow;

    [Header("Speed")]
    public KeyCode speedUpKey = KeyCode.W;
    public KeyCode speedUpAltKey = KeyCode.UpArrow;
    public KeyCode slowDownKey = KeyCode.S;
    public KeyCode slowDownAltKey = KeyCode.DownArrow;

    [Header("Combat")]
    // Shoot Mechanics 
    public KeyCode shootKey = KeyCode.Mouse0;


    public override void MakeDecisions()
    {
        if (pawn != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pawn.AimAt(mouseWorld);

            if (Input.GetKeyDown(moveLeftKey) || Input.GetKeyDown(moveLeftAltKey))
            {
                pawn.MoveLeft();
            }
            if (Input.GetKeyDown(moveRightKey) || Input.GetKeyDown(moveRightAltKey))
            {
                pawn.MoveRight();
            }

            if (Input.GetKey(speedUpKey) || Input.GetKey(speedUpAltKey))
            {
                pawn.SpeedUp();
            }
            else if (Input.GetKey(slowDownKey) || Input.GetKey(slowDownAltKey))
            {
                pawn.SlowDown();
            }

            //Shoot key press 
            if (Input.GetKeyDown(shootKey))
            {
                //telling pawn to shoot
                pawn.Shoot();
            }
        }
        
    }

    //Registers this controller with the GameManager so it's able to hand controls to the new truck
    public override void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.playerController = this;
        }
    }

    // Checks for key presses every frame
    public override void Update()
    {
        MakeDecisions();
    }
}
