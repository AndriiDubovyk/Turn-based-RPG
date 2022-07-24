using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public Tilemap tilemap;

    private Vector3Int? destinationTileGridPos = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(destinationTileGridPos != null)
        {
            Vector3 destinationPoint = new Vector3(destinationTileGridPos.Value.x * 0.16f + 0.08f, destinationTileGridPos.Value.y * 0.16f + 0.08f, 0);
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint, moveSpeed * Time.deltaTime);

            if(destinationPoint == transform.position)
            {
                destinationTileGridPos = null;
            }
        }

        Vector3Int currentPlayerGridPos = tilemap.WorldToCell(transform.position);
    }

    public void setMoveDestination(Vector3Int newDestinationTileGridPos)
    {
        destinationTileGridPos = newDestinationTileGridPos;
    }
}
