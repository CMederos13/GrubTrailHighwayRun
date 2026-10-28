using UnityEngine;
using UnityEngine.UI;


//Tracks Health, takes damage, updates the health bar, and triggers death
public class Health : MonoBehaviour
{
    [Header("Health Values")]
    public float maxHealth;
    public float currentHealth;
    
   

    [Header("Health UI")]
    public Image healthBar; 
    [Header("Damage Numbers")]
    public GameObject damageNumberPrefab;

    public Transform damageNumberParent;  // UI Spot for the player's number (empty = above the object)

    private Death death;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        death = GetComponent<Death>();

        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }


    public void TakeDamage(float damageAmount)
    {
        //If I'm already dead. Ignore any more damage
        //This ends the method right there and skips everything below it; ex. a dead asteroid hit by a second bullet in the same frame
        if (currentHealth <= 0)
        {
            return;
        }

        // subtract damage from my current health

        currentHealth -= damageAmount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Plays the damage sound through the GameManager's SFX Player
        if (currentHealth > 0)
        {
            GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.takingDamageSound);
        }

        //shows the floating damage number

        SpawnDamageNumber(damageAmount);

        //HealthBar
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        //Death 
        if (currentHealth <= 0)
        {
            if (death != null)
            {
                death.Die();
            }
        }

    }

    // Shows the floating damage number
    private void SpawnDamageNumber(float amount)
    {
        if (damageNumberPrefab == null)
        {
            return;
        }

        GameObject newNumber;
        if (damageNumberParent != null)
        {
            // spawns inside the HUD Health Bar, Position by the anchor
            newNumber = Instantiate(damageNumberPrefab, damageNumberParent);
        }
        else
        {
            // spawns at the object's position, a little above it
            newNumber = Instantiate(damageNumberPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        //had to look this up in the unity manual along with reddit
        // also used lecture 6.2 
        // Takes the new number how much damage to display 

        DamageGhostNumbers numberscript = newNumber.GetComponent<DamageGhostNumbers>();
        if (numberscript != null)
        {
            numberscript.SetDamage(amount);
        }
    }
    public void Heal(float healAmount)
    {
        // Healing Message
        Debug.Log("Patching up!");
        // Adding Health to Current Health

        currentHealth += healAmount;

        // if health > max then set health to max
        //if (currentHealth > maxHealth)
        // {
        //   currentHealth = maxHealth;
        // setting the max health for healing 

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);


        // if health > max then set health to max
        //if (currentHealth > maxHealth)
        // {
        //   currentHealth = maxHealth;
        //}
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }


    }


    public void Die()
    {
        Debug.Log("Wasted!");
    }
}