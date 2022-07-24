using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public Tilemap uiOverlayTilemap;
    public Tile selectionTile;
    Vector3Int? selectedTileGridPos = null;

    // Start is called before the first frame update
    void Start()
    {
        
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

            // Select new clicked tile
            selectedTileGridPos = currentCellGridPos;
            uiOverlayTilemap.SetTile(currentCellGridPos, selectionTile);
        }
    }
}
