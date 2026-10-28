 using UnityEngine;

//Base class for all AI Controllers; Holds what every AI needs
public abstract class AIController : Controller
{
    protected Transform target; // What this AI is Chasing

    protected void FindPlayerTarget() // finds the player's current ship (it changes each time the player respawns) 
    {
        target = null;

        if (GameManager.instance.playerController != null && GameManager.instance.playerController.pawn != null)
        {
            target = GameManager.instance.playerController.pawn.transform;
        }
    }

}
