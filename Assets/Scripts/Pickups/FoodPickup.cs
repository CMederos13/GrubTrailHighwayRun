using UnityEngine;

public class FoodPickup : Pickup
{
    [SerializeField] private int points = 25;

    protected override void Apply(GameObject truck)
    {
        GameManager.instance.score = GameManager.instance.score + points;
    }
}