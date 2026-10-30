using UnityEngine;

//Scrolls the road and grass down at the truck's speed and tracks distance
public class RoadScroller : MonoBehaviour
{
    [Header("Road Pieces")]
    public Sprite roadSprite;
    public Sprite shoulderSprite;
    public int tileCount = 3;
    public float tileHeight = 17.98f;
    public float shoulderOffset = 10.265f;

    [Header("Speed")]
    public TruckPawn truck;
    public float distanceTraveled = 0f;

    private Transform[] tiles;

    public void Start()
    {
        tiles = new Transform[tileCount];
        // Lays out the road tiles stacked on top of each other
        for (int i = 0; i < tileCount; i++)
        {
            tiles[i] = MakeTile(i * tileHeight);
        }
    }

    public void Update()
    {
        // Stops scrolling once the truck is destroyed 
        if (truck == null)
        {
            return;
        }

        float moveAmount = truck.currentSpeed * Time.deltaTime;
        distanceTraveled = distanceTraveled + moveAmount;

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].position = tiles[i].position + Vector3.down * moveAmount;
            //Moves a tile that went off the bottom back to the top
            if (tiles[i].position.y <= -tileHeight)
            {
                tiles[i].position = tiles[i].position + Vector3.up * tileHeight * tileCount;
            }
        }
    }

    // Builds one row: road in the middle, grass on both sides
    private Transform MakeTile(float y)
    {
        GameObject tile = new GameObject("RoadTile");
        tile.transform.parent = transform;
        tile.transform.position = new Vector3(0f, y, 0f);
        AddPiece(tile.transform, roadSprite, 0f);
        AddPiece(tile.transform, shoulderSprite, -shoulderOffset);
        AddPiece(tile.transform, shoulderSprite, shoulderOffset);
        return tile.transform;
    }

    // Creates one sprite piece (road or grass) inside a row 
    private void AddPiece(Transform parent, Sprite sprite, float x)
    {
        GameObject piece = new GameObject(sprite.name);
        piece.transform.parent = parent;
        piece.transform.localPosition = new Vector3(x, 0f, 0f);
        SpriteRenderer sr = piece.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = -10;
    }
}