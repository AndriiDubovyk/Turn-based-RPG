using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject grid;

    public Tile selectionTile;
    public Tile attackSelectionTile;
    public Tile pathMarkTile;

    public TextMeshProUGUI healtBar;

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
            GameObject clickedUnit = gridManager.GetUnitAtCell(clickedCell);
            if (clickedUnit == null)
            {
                if (unitController.GetMovementPath() != null && clickedCell == unitController.GetMovementPath().Last() && !pathConfirmed)
                {
                    pathConfirmed = true;
                    unitController.ConfirmTurn();
                }
                else
                {
                    pathConfirmed = false;
                    unitController.SetMovementPathTo(clickedCell);
                    ShowOverlayMarks();
                }
            } 
            else
            {
                if(clickedUnit != gameObject && unitController.CanAttack(clickedUnit)) // player can't attack himself
                {
                    if(unitController.GetEnemyTarget() == clickedUnit)
                    {
                        unitController.ConfirmTurn();
                    }
                    else
                    {
                        unitController.SetEnemyTarget(clickedUnit);
                        ShowOverlayMarks();
                    }
                }
            }
        }
    }


    public void ShowOverlayMarks()
    {
        gridManager.uiOverlayTilemap.ClearAllTiles();

        GameObject enemyTarget = unitController.GetEnemyTarget();
        List<Vector3Int> movementPath = unitController.GetMovementPath();    

        if(enemyTarget!=null)
        {
            Vector3Int targetCell = enemyTarget.GetComponent<UnitController>().GetPositionOnGrid();
            gridManager.uiOverlayTilemap.SetTile(targetCell, attackSelectionTile);
        } 
        else if (movementPath != null && movementPath.Count > 0)
        {
            for (int i = 0; i < movementPath.Count; i++)
            {
                gridManager.uiOverlayTilemap.SetTile(movementPath[i], pathMarkTile);
            }
            gridManager.uiOverlayTilemap.SetTile(movementPath.Last(), selectionTile);
        }
    }

    public void UpdateHealthBar()
    {
        healtBar.text = "HP: "+GetComponent<CombatUnit>().GetCurrentHP();
    }

}
