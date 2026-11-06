using UnityEngine;

// Food (burger, taco, burrito, onion rings) inherits from Pickup and adds points
public class FoodPickup : Pickup
{
    [SerializeField] private int points = 25;

    // Adds this food's points to the score
    protected override void Apply(GameObject truck)
    {
        GameManager.instance.score = GameManager.instance.score + points;
    }
}
