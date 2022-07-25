using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public Tilemap groundTilemap;
    public Tilemap uiOverlayTilemap;
    public Tilemap collidersTilemap;

    public Tile selectionTile;
    public Tile pathMarkTile;

    public List<Vector3Int> movementPath;

    private float tileSize = 1f;
    private float xPivot = 0.5f;
    private float yPivot = 0.5f;
    private int pathfindingWidth = 40;
    private int pathfindingHeight = 22;

    private bool canMove = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove && movementPath != null && movementPath.Count > 0)
        {
            // Finx next cell point
            Vector3 nextCellPoint = new Vector3(movementPath[0].x * tileSize + tileSize * xPivot, movementPath[0].y * tileSize + tileSize * yPivot, 0);

            // Move to next cell point
            transform.position = Vector3.MoveTowards(transform.position, nextCellPoint, moveSpeed * Time.deltaTime);

            // Check if we reach next cell
            if (nextCellPoint == transform.position)
            {
                // Remove this point from path
                ClearNextMovementPathMark();

                // If it was last point it means we reach the destination
                if(movementPath.Count == 0)
                {
                    ClearMovementPath();  
                    canMove = false;
                }
            }

        }

    }

    public void SetMovementPathTo(Vector3Int destinationCell)
    {
        ClearMovementPath();
        Vector3Int currentPlayerGridPos = groundTilemap.WorldToCell(transform.position);
        int offsetX = currentPlayerGridPos.x - pathfindingWidth / 2;
        int offsetY = currentPlayerGridPos.y - pathfindingHeight / 2;
        Pathfinder pf = new Pathfinder(offsetX, offsetY, pathfindingWidth, pathfindingHeight, collidersTilemap);
        movementPath = pf.GetPath(currentPlayerGridPos.x - offsetX, currentPlayerGridPos.y - offsetY, destinationCell.x - offsetX, destinationCell.y - offsetY);
        ShowPathMarks();
    }

    private void ShowPathMarks()
    {
        if(movementPath != null) {
            for (int i = 0; i < movementPath.Count - 1; i++)
            {
                uiOverlayTilemap.SetTile(movementPath[i], pathMarkTile);
            }
            uiOverlayTilemap.SetTile(movementPath.Last(), selectionTile);
        }
    }

    private void ClearMovementPath()
    {
        if (movementPath != null)
        {
            for (int i = 0; i < movementPath.Count; i++)
            {
                uiOverlayTilemap.SetTile(movementPath[i], null);
            }
        }
        movementPath = null;
    }

    private void ClearNextMovementPathMark()
    {
        if (movementPath != null && movementPath.Count > 0)
        {
            uiOverlayTilemap.SetTile(movementPath[0], null);
            movementPath.RemoveAt(0);
        }
    }

    public void StartMoving()
    {
        canMove = true;
    }

    private Vector3Int GetPosition()
    {
        return groundTilemap.WorldToCell(transform.position); ;
    }
}
