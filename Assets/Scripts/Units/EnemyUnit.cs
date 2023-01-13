using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnit : Unit
{
    private HealthBar healthBar;
    private TurnManager tm;
    [SerializeField]
    private FloatingText floatingExp;

    [SerializeField]
    private float atackTimeWait;
    private float timeToAttack = float.MaxValue;

    protected override void Awake()
    {
        base.Awake();
        tm = GameObject.Find("GameHandler").GetComponent<TurnManager>();
        tm.AddEnemy(gameObject);
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealth(GetCurrentHP(), GetMaxHP());
    }

    protected override void Update()
    {
        if (state == State.IsMakingTurn)
        {
            if (attackTarget != null)
            {
                if (timeToAttack == float.MaxValue)
                {
                    timeToAttack = atackTimeWait;
                    
                }
                else
                {
                    timeToAttack -= Time.deltaTime;
                    Debug.Log($"{timeToAttack}");
                }

                if(timeToAttack<=0)
                {
                    timeToAttack = float.MaxValue;
                    AttackUpdate();
                }
            }
            else if (movementPath != null && movementPath.Count > 0)
            {
                MoveUpdate();
            }

        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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

        UnitsPathfinder pf = new UnitsPathfinder(offsetX, offsetY, pathfindingXMaxDistance * 2 + 1, pathfindingYMaxDistance * 2 + 1, gridManager.collidersTilemap, otherUnitsCells);
        List<Coords> movementPathCoords = pf.GetPath(currentCell.x - offsetX, currentCell.y - offsetY, destinationCell.x - offsetX, destinationCell.y - offsetY);
        if (movementPathCoords == null) return;
        movementPath = movementPathCoords.ConvertAll<Vector3Int>(c => new Vector3Int(c.X, c.Y));
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

    public override void Die()
    {
        GameObject.Find("Player").GetComponent<PlayerUnit>().AddExp(unitData.expReward);
        if (canvas != null && floatingExp != null)
        {
            FloatingText fe = Instantiate(floatingExp, transform.position, Quaternion.identity);
            fe.SetParentCanvas(canvas);
            fe.SetText($"+{unitData.expReward} EXP");
        }
        base.Die();
    }
}
