using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public int maxAttackDistance = 1;

    private GridManager gridManager;
    private List<Vector3Int> movementPath;
    private GameObject enemyTarget;
    private CombatUnit combatUnit;

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
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        combatUnit = GetComponent<CombatUnit>();
        movementPath = null;
        state = State.IsThinking;
        enemyTarget = null;
    }

    public void SetMovementPathTo(Vector3Int destinationCell)
    {
        Vector3Int currentCell = GetPositionOnGrid();
        int offsetX = currentCell.x - pathfindingXMaxDistance;
        int offsetY = currentCell.y - pathfindingYMaxDistance;
        // We can't reach this cell anyway so we can just set movementPath to null
        if(Math.Abs(destinationCell.x - currentCell.x) > pathfindingXMaxDistance
            || Math.Abs(destinationCell.y - currentCell.y) > pathfindingYMaxDistance)
        {
            movementPath = null;
            return;
        }
        List<Vector3Int> otherUnitsCells = GetOtherUnitsCells();
        Pathfinder pf = new Pathfinder(offsetX, offsetY, pathfindingXMaxDistance * 2 + 1, pathfindingYMaxDistance * 2 + 1, gridManager.collidersTilemap, otherUnitsCells);
        movementPath = pf.GetPath(currentCell.x - offsetX, currentCell.y - offsetY, destinationCell.x - offsetX, destinationCell.y - offsetY);
        // Enemy need path to player neighbour's cell (not to player's cell)
        if (tag=="Enemy" && movementPath!=null)
        {
            movementPath.RemoveAt(movementPath.Count - 1);
            if (movementPath.Count == 0) movementPath = null;
        }
    }

    private List<Vector3Int> GetOtherUnitsCells()
    {
        List<Vector3Int> otherUnitsCells = new List<Vector3Int>();

        /* 
         * We don't need to add player. 
         * Enemies must not consider player cell as occupied to get path to player.
         * And player anyway can't walk again on current cell
         */
        List<GameObject> enemies = GameObject.Find("GameHandler").GetComponent<TurnManager>().enemies;
        for(int i=0; i<enemies.Count; i++)
        {
            if (gameObject != enemies[i]) otherUnitsCells.Add(enemies[i].GetComponent<UnitController>().GetPositionOnGrid());
        }

        return otherUnitsCells;
    }

    public void SetEnemyTarget(GameObject target)
    {
        enemyTarget = target;
    }

    public bool CanAttack(GameObject enemy)
    {
        Vector3Int cell = GetPositionOnGrid();
        Vector3Int enemyCell = enemy.GetComponent<UnitController>().GetPositionOnGrid();
        // Manhattan Distance
        int distance = Math.Abs(cell.x - enemyCell.x) + Math.Abs(cell.y - enemyCell.y);
        return distance <= maxAttackDistance;
    }

    public void ConfirmTurn()
    {
        state = State.IsMakingTurn;
        // Skip turn. Temporary solution. TODO
        if (enemyTarget==null &&movementPath == null) state = State.IsWaiting;
    }


    public Vector3Int GetPositionOnGrid()
    {
        return gridManager.groundTilemap.WorldToCell(transform.position);
    }

    public List<Vector3Int> GetMovementPath()
    {
        return movementPath;
    }

    public GameObject GetEnemyTarget()
    {
        return enemyTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.IsMakingTurn)
        {
            if(enemyTarget!=null)
            {
                Attack();
            }
            else if (movementPath != null && movementPath.Count > 0)
            {
                Move();
            }
            
        }
    }

    private void Attack()
    {
        combatUnit.Attack(enemyTarget.GetComponent<CombatUnit>());
        enemyTarget = null;
        if (tag == "Player") GetComponent<PlayerController>().UpdateOverlayMarks();
        else if (tag == "Enemy") GameObject.Find("Player").GetComponent<PlayerController>().UpdateHealthBar();
        state = State.IsWaiting;
    }

    private void Move()
    {
        // Find next cell point
        Vector3 nextCellPoint = new Vector3(movementPath[0].x * gridManager.tileSize + gridManager.tileSize * gridManager.xTilePivot, movementPath[0].y * gridManager.tileSize + gridManager.tileSize * gridManager.yTilePivot, 0);

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
            if (tag == "Player") GetComponent<PlayerController>().UpdateOverlayMarks();
            state = State.IsWaiting;
        }
    }
}
