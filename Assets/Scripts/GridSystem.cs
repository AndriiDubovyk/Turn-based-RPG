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
                playerController.StartMoving();
                selectedTileGridPos = null;
            } 
            else if (collidersTilemap.GetTile(currentCellGridPos) == null)
            {
                // Select new tile if it don't have objects that block moving
                selectedTileGridPos = currentCellGridPos;
                playerController.SetMovementPathTo(currentCellGridPos);
            }
            
        }
    }

    
}
