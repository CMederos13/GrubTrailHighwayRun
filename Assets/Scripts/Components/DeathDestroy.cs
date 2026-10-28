using UnityEngine;

public class DeathDestroy : Death
{
    public override void Die()
    {
        Destroy(gameObject);
    }
    public override void Start()
    {

    }
    public override void Update()
    {
        
    }

}
