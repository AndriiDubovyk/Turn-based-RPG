using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public float moveSpeed = 10f;


    public GameObject grid;

    private GridManager gridManager;
    private List<Vector3Int> movementPath;

    public State state;
    public enum State
    {
        IsThinking, // think about own turn
        IsMakingTurn, // turn is in process
        IsWaiting // waiting opponent's turn end
    }

    
    public int pathfindingXMaxDistance = 22;
    public int pathfindingYMaxDistance = 14;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = grid.GetComponent<GridManager>();
        movementPath = null;
        state = State.IsThinking;
    }

    public void SetMovementPathTo(Vector3Int destinationCell)
    {
        Vector3Int currentCell = GetPositionOnGrid();
        int offsetX = currentCell.x - pathfindingXMaxDistance;
        int offsetY = currentCell.y - pathfindingYMaxDistance;
        if(Math.Abs(destinationCell.x - currentCell.x) > pathfindingXMaxDistance
            || Math.Abs(destinationCell.y - currentCell.y) > pathfindingYMaxDistance)
        {
            movementPath = null;
            return;
        }
        Pathfinder pf = new Pathfinder(offsetX, offsetY, pathfindingXMaxDistance * 2 + 1, pathfindingYMaxDistance * 2 + 1, gridManager.collidersTilemap);
        movementPath = pf.GetPath(currentCell.x - offsetX, currentCell.y - offsetY, destinationCell.x - offsetX, destinationCell.y - offsetY);
        // Enemy need path to player neighbour's cell (not to player's cell)
        if(tag=="Enemy" && movementPath!=null)
        {
            movementPath.RemoveAt(movementPath.Count - 1);
            if (movementPath.Count == 0) movementPath = null;
        }
    }

    public void ConfirmTurn()
    {
        state = State.IsMakingTurn;

        // Temporary solution. TODO
        if (movementPath == null) state = State.IsWaiting;
    }


    public Vector3Int GetPositionOnGrid()
    {
        return gridManager.groundTilemap.WorldToCell(transform.position);
    }

    public List<Vector3Int> GetMovementPath()
    {
        return movementPath;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.IsMakingTurn)
        {
            if (movementPath == null || movementPath.Count == 0) return;
            // Find next cell point
            Vector3 nextCellPoint = new Vector3(movementPath[0].x * gridManager.tileSize + gridManager.tileSize * gridManager.xTilePivot, movementPath[0].y *  gridManager.tileSize + gridManager.tileSize * gridManager.yTilePivot, 0);

            // Move to next cell point
            transform.position = Vector3.MoveTowards(transform.position, nextCellPoint, moveSpeed * Time.deltaTime);

            // Check if we reach next cell and if so stop moving
            if (nextCellPoint == transform.position)
            {
                movementPath.RemoveAt(0);
                if (movementPath.Count == 0)
                {
                    movementPath = null;
                }
                if (tag == "Player") GetComponent<PlayerController>().ShowPathMarks();
                state = State.IsWaiting;
            }
        }
    }
}
