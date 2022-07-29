using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    private HealthBar healthBar;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealth(GetCurrentHP(), GetMaxHP());
    }

    public override void SetMovementPathTo(Vector3Int destinationCell)
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
        // Player cell must be considered as walkable for enemy for pathifinding
        otherUnitsCells.Remove(gridManager.GetPlayerCell());

        Pathfinder pf = new Pathfinder(offsetX, offsetY, pathfindingXMaxDistance * 2 + 1, pathfindingYMaxDistance * 2 + 1, gridManager.collidersTilemap, otherUnitsCells);
        movementPath = pf.GetPath(currentCell.x - offsetX, currentCell.y - offsetY, destinationCell.x - offsetX, destinationCell.y - offsetY);
        if (movementPath != null)
        {
            movementPath.RemoveAt(movementPath.Count - 1);
            if (movementPath.Count == 0) movementPath = null;
        }
    }

    public override void Attack(Unit another)
    {
        base.Attack(another);
    }

    public override void UpdateHealthBar()
    {
        healthBar.UpdateHealth(currentHP, maxHP);
    }
}
