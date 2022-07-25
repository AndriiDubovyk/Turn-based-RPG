using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public Tilemap groundTilemap;
    public Tilemap collidersTilemap;

    private Vector3Int? destinationTileGridPos = null;

    private float tileSize = 1f;
    private float xPivot = 0.5f;
    private float yPivot = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(destinationTileGridPos != null)
        {
            Vector3 destinationPoint = new Vector3(destinationTileGridPos.Value.x * tileSize + tileSize * xPivot, destinationTileGridPos.Value.y * tileSize + tileSize * yPivot, 0);
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint, moveSpeed * Time.deltaTime);

            if(destinationPoint == transform.position)
            {
                destinationTileGridPos = null;
            }
        }

        Vector3Int currentPlayerGridPos = groundTilemap.WorldToCell(transform.position);
    }

    public void SetMoveDestination(Vector3Int newDestinationTileGridPos)
    {
        
        destinationTileGridPos = newDestinationTileGridPos;
    }
}
