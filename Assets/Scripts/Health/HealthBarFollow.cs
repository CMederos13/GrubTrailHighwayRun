using UnityEngine;


// keeps the zombie's health bar upright and above it, even when the zombie rotates 
public class HealthBarFollow : MonoBehaviour
{
    [Header("Position")]
    public Vector3 offset = new Vector3(0, 0.385f, 0); // how far above the enemy the bar sits
   
    private Transform owner; // The zombie this bar belongs to

    private void Start()
    {
        owner = transform.parent;
    }

    // LateUpdate runs after the enemy has moved and turned this frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity; //  ALWAYS level
        transform.position = owner.position + offset; // ALWAYS above the zombie
    }
}
