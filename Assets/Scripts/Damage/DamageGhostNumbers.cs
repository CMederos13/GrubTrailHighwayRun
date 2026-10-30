using UnityEngine;
using TMPro;
public class DamageGhostNumbers : MonoBehaviour
{
    //A Floating number that shows damage taken, drifts up, then destroy itself
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float floatSpeed = 1f; // unit per second upward
    [SerializeField] private float lifespan = 1f; // seconds before it's destroyed
    
    void Start()
    {
        //TODO 1: Destroy this gameObject after "lifespan' ends
        // use same second Destroy() parameter for delay 
        Destroy(gameObject, lifespan);
    }

    // Health is called right after spawning the number 
    public void SetDamage(float amount)
    {

        //TODO 2: if damageText isn't null, set text to "-"
        if (damageText != null)
        {
            damageText.text = "-" + amount;
        }
    }
    
    void Update()
    {
        //TODO 3: Move up a little each fram 

        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }
}
