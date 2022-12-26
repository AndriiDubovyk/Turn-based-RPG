using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected GridManager gridManager;

    // Stats
    [SerializeField]
    protected int maxHP;
    [SerializeField]
    protected int attack;
    [SerializeField]
    protected int maxAttackDistance;

    protected int currentHP;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    protected int pathfindingXMaxDistance;
    [SerializeField]
    protected int pathfindingYMaxDistance;

    // Data for turn
    protected List<Vector3Int> movementPath;
    protected Unit attackTarget;

    // States
    protected State state;
    public enum State
    {
        IsThinking, // think about own turn
        IsMakingTurn, // turn is in process
        IsWaiting // waiting opponent's turn end
    }

    protected virtual void Start()
    {
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        movementPath = null;
        attackTarget = null;
        state = State.IsThinking;
        currentHP = maxHP;
    }

    void Update()
    {
        if (state == State.IsMakingTurn)
        {
            if (attackTarget != null)
            {
                AttackUpdate();
            }
            else if (movementPath != null && movementPath.Count > 0)
            {
                MoveUpdate();
            }

        }
    }

    private void AttackUpdate()
    {
        Attack(attackTarget);
        attackTarget = null;
        state = State.IsWaiting;
    }

    protected virtual void MoveUpdate()
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
            state = State.IsWaiting;
        }
    }

    public virtual void Attack(Unit another)
    {
        Debug.Log(tag + " attack " + another.tag + " with " + attack + " dmg");
        another.TakeDamage(this.attack);
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        if (currentHP < 0) currentHP = 0;
        Debug.Log(tag + " was attacked with " + damageAmount + " dmg. Current HP: " + currentHP);
        UpdateHealthBar();
    }

    public virtual void UpdateHealthBar() {}

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    public virtual void SetMovementPathTo(Vector3Int destinationCell)
    {
        Vector3Int currentCell = GetCell();
        int offsetX = currentCell.x - pathfindingXMaxDistance;
        int offsetY = currentCell.y - pathfindingYMaxDistance;
        // We can't reach this cell anyway so we can just set movementPath to null
        if (Math.Abs(destinationCell.x - currentCell.x) > pathfindingXMaxDistance
            || Math.Abs(destinationCell.y - currentCell.y) > pathfindingYMaxDistance)
        {
            movementPath = null;
            return;
        }
        List<Vector3Int> otherUnitsCells = gridManager.GetOccupiedCells();
        otherUnitsCells.Remove(this.GetCell());
        UnitsPathfinder pf = new UnitsPathfinder(offsetX, offsetY, pathfindingXMaxDistance * 2 + 1, pathfindingYMaxDistance * 2 + 1, gridManager.collidersTilemap, otherUnitsCells);
        List<Coords> movementPathCoords = pf.GetPath(currentCell.x - offsetX, currentCell.y - offsetY, destinationCell.x - offsetX, destinationCell.y - offsetY);
        movementPath = movementPathCoords.ConvertAll<Vector3Int>(c => new Vector3Int(c.X, c.Y));
    }

    public void SetAttackTarget(Unit target)
    {
        attackTarget = target;
    }

    public Vector3Int GetCell()
    {
        return gridManager.groundTilemap.WorldToCell(transform.position);
    }

    public List<Vector3Int> GetMovementPath()
    {
        return movementPath;
    }

    public Unit GetAttackTarget()
    {
        return attackTarget;
    }

    public State GetState()
    {
        return state;
    }

    public void SetState(State newState)
    {
        state = newState;
    }

    public bool CanAttack(Unit enemy)
    {
        if (enemy == this) return false;
        Vector3Int cell = GetCell();
        Vector3Int enemyCell = enemy.GetCell();
        // Manhattan Distance
        int distance = Math.Abs(cell.x - enemyCell.x) + Math.Abs(cell.y - enemyCell.y);
        return distance <= maxAttackDistance;
    }

    public void ConfirmTurn()
    {
        state = State.IsMakingTurn;
        // Skip turn. Temporary solution. TODO
        if (attackTarget == null && movementPath == null) state = State.IsWaiting;
    }
}
