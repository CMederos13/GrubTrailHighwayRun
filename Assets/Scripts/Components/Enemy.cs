using UnityEngine;


public class Enemy : MonoBehaviour
{
   
    void Awake()
    {
        GameManager.instance.enemyList.Add(this);

        //Get this enemy's Audio Source and start Its zombie horde sound
        AudioSource humSource = GetComponent<AudioSource>();
        if(humSource != null)
        {
            humSource.clip = GameManager.instance.zombieHordeSound;
            humSource.Play();
        }
    }
    void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.enemyList.Remove(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
    