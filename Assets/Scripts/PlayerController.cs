using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public GameObject grid;

    public Tile selectionTile;
    public Tile pathMarkTile;

    private GridManager gridManager;
    private UnitController unitController;

    public bool pathConfirmed;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = grid.GetComponent<GridManager>();
        unitController = GetComponent<UnitController>();
        pathConfirmed = false;
    }

    public void SelectDestinationCell()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 worldClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = gridManager.groundTilemap.WorldToCell(worldClickPos);
            Debug.Log("Click");
            if (unitController.GetMovementPath() != null && clickedCell == unitController.GetMovementPath().Last() && !pathConfirmed)
            {
                pathConfirmed = true;
                unitController.ConfirmTurn();
            }
            else
            {
                pathConfirmed = false;
                Debug.Log("Set path");
                unitController.SetMovementPathTo(clickedCell);
                ShowPathMarks();
            }
        }
    }


    public void ShowPathMarks()
    {
        gridManager.uiOverlayTilemap.ClearAllTiles();
        List<Vector3Int> movementPath = unitController.GetMovementPath();

        if (movementPath != null && movementPath.Count > 0)
        {
            for (int i = 0; i < movementPath.Count; i++)
            {
                gridManager.uiOverlayTilemap.SetTile(movementPath[i], pathMarkTile);
            }
            gridManager.uiOverlayTilemap.SetTile(movementPath.Last(), selectionTile);
        }
    }

}
