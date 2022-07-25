using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public GameObject player;
    public Tilemap groundTilemap;
    public Tilemap uiOverlayTilemap;
    public Tilemap collidersTilemap;
    public Tile selectionTile;
    public Tile pathMarkTile;

    private Vector3Int? selectedTileGridPos;
    private List<Vector3Int> movementPath;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        selectedTileGridPos = null;
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 worldClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int currentCellGridPos = uiOverlayTilemap.WorldToCell(worldClickPos);

            

            // Remove previous selection if we had one
            if(selectedTileGridPos != null)
            {
                uiOverlayTilemap.SetTile(selectedTileGridPos.Value, null);
            }

            if (currentCellGridPos == selectedTileGridPos)
            {
                // Move character to the selected grid
                playerController.SetMoveDestination(currentCellGridPos);
                selectedTileGridPos = null;
            } 
            else if (collidersTilemap.GetTile(currentCellGridPos) == null)
            {
                // Select new tile if it don't have objects that block moving
                selectedTileGridPos = currentCellGridPos;
                SetPlayerPath(currentCellGridPos);
                uiOverlayTilemap.SetTile(currentCellGridPos, selectionTile);
            }
            
        }

        // If player reach destination clear path marks
        if(movementPath != null)
        {
            Vector3Int currentPlayerGridPos = groundTilemap.WorldToCell(player.transform.position);
            if(movementPath.Last()==currentPlayerGridPos)
            {
                ClearPathMarks();
            }
        }
    }

    private void SetPlayerPath(Vector3Int destinationTileGridPos)
    {
        ClearPathMarks();

        int searchPathZoneWidth = 30;
        int searchPathZoneHeight = 20;

        Vector3Int currentPlayerGridPos = groundTilemap.WorldToCell(player.transform.position);
        int offsetX = currentPlayerGridPos.x - searchPathZoneWidth/2;
        int offsetY = currentPlayerGridPos.y - searchPathZoneHeight/2;
        Pathfinder pf = new Pathfinder(offsetX, offsetY, searchPathZoneWidth, searchPathZoneHeight, collidersTilemap);
        movementPath = pf.GetPath(currentPlayerGridPos.x - offsetX, currentPlayerGridPos.y - offsetY, destinationTileGridPos.x - offsetX, destinationTileGridPos.y - offsetY);
        for (int i = 0; i < movementPath.Count; i++)
        {
            uiOverlayTilemap.SetTile(movementPath[i], pathMarkTile);
        }
    }

    private void ClearPathMarks()
    {
        if (movementPath != null)
        {
            for (int i = 0; i < movementPath.Count; i++)
            {
                uiOverlayTilemap.SetTile(movementPath[i], null);
            }
        }
    }
}
