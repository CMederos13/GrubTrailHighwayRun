using UnityEngine;

// Handles the player's truck dying; explosion sound, camera takes over
public class DeathPawn : Death
{

   
    public override void Die()
    {
        // Play the explosion sound through the GameManager's SFX player
        GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.explosionSound);

        
        //Hands over Listening priority to the camera so sounds can still be heard after destroyed
        AudioListener truckListener = GetComponent<AudioListener>();
        
        if (truckListener != null)
        {
            truckListener.enabled = false;
        }

        GameManager.instance.cameraListener.enabled = true;

        //Destroying the truck lets the GameManager see it's gone and end the game
        Destroy(gameObject);
        
    }

    public override void Start()
    {
       
    }

    public override void Update()
    {

    }
}

   