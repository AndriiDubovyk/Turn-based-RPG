using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public GameObject player;
    public Tilemap uiOverlayTilemap;
    public Tile selectionTile;

    private Vector3Int? selectedTileGridPos;
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
            } else
            {
                // Select new tile
                selectedTileGridPos = currentCellGridPos;
                uiOverlayTilemap.SetTile(currentCellGridPos, selectionTile);
            }
            
        }
    }
}
