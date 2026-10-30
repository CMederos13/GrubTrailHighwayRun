using UnityEngine;

public class ZombieAIController : AIController
{
    public override void Start()
    {
        if (pawn == null)
        {
            pawn = GetComponent<Pawn>();
        }
    }

    public override void Update()
    {
        MakeDecisions();
    }

    public override void MakeDecisions()
    {
        FindPlayerTarget();

        ZombiePawn zombie = pawn as ZombiePawn;
        if (target != null && zombie != null)
        {
            zombie.MoveToward(target.position);
        }
    }
}