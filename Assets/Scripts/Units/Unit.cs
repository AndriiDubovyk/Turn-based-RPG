using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class Unit : MonoBehaviour
{
    protected GridManager gridManager;


    public UnitData unitData;

    protected int maxHP;
    protected int attack;
    protected int defense;
    protected int maxAttackDistance;
    protected float moveSpeed;
    protected int pathfindingXMaxDistance;
    protected int pathfindingYMaxDistance;

    protected int currentHP;

    // Data for turn
    protected List<Vector3Int> movementPath;
    protected Unit attackTarget;

    protected Animator animator;

    protected CrossSceneAudioManager audioManager;
    protected VillageInfo villageInfo;

    [SerializeField]
    protected GameObject blood;
    [SerializeField]
    protected GameObject canvas;
    [SerializeField]
    protected FloatingText floatingDamage;

    // States
    protected State state;
    public enum State
    {
        IsThinking, // think about own turn
        IsMakingTurn, // turn is in process
        IsWaiting // waiting opponent's turn end
    }

    protected virtual void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 0) audioManager = objs[0].GetComponent<CrossSceneAudioManager>();

        objs = GameObject.FindGameObjectsWithTag("village_info");
        if (objs.Length > 0) villageInfo = objs[0].GetComponent<VillageInfo>();

        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        maxHP = unitData.maxHP;
        attack = unitData.attack;
        defense = unitData.defense;
        maxAttackDistance = unitData.maxAttackDistance;
        moveSpeed = unitData.moveSpeed;
        currentHP = maxHP;
        pathfindingXMaxDistance = unitData.pathfindingXMaxDistance;
        pathfindingYMaxDistance = unitData.pathfindingYMaxDistance;
        gameObject.GetComponent<SpriteRenderer>().sprite = unitData.sprite;
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        movementPath = null;
        attackTarget = null;
        SetState(State.IsThinking);
    }

    public void SetHealth(int health)
    {
        currentHP = health;
        UpdateHealthBar();
    }

    protected virtual void Update()
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

    protected void AttackUpdate()
    {
        Attack(attackTarget);
        attackTarget = null;
        SetState(State.IsWaiting);
    }

    protected virtual void MoveUpdate()
    {
        // Find next cell point
        Vector3 nextCellPoint = new Vector3(movementPath[0].x * gridManager.tileSize + gridManager.tileSize * gridManager.xTilePivot, movementPath[0].y * gridManager.tileSize + gridManager.tileSize * gridManager.yTilePivot, 0);

        // Flip sprite if requires   
        FlipToPoint(nextCellPoint);

        // Move to next cell point
        transform.position = Vector3.MoveTowards(transform.position, nextCellPoint, moveSpeed * Time.deltaTime);

        // Check if we reach next cell and if so stop moving
        if (nextCellPoint == transform.position)
        {
            gridManager.UpdateVisibility();
            if(unitData.name=="Player") GameObject.Find("UICanvas").GetComponent<UI>().UpdateItemTakeScrollItems();
            movementPath.RemoveAt(0);
            if (movementPath.Count == 0)
            {
                movementPath = null;
            }
            SetState(State.IsWaiting);
        }
    }

    protected void FlipToPoint(Vector3 point)
    {
        // Flip sprite if requires   
        if (point.x > gameObject.transform.position.x)
        {
            gameObject.transform.localScale = new Vector3(1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if (point.x < transform.position.x)
        {
            gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }

    }

    public virtual void Attack(Unit another)
    {
        // Flip sprite if requires   
        FlipToPoint(another.transform.position);
        if(animator!=null) animator.SetTrigger("AttackTrigger");
        another.TakeDamage(this.attack, this);
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    public int GetAttack()
    {
        return attack;
    }

    public int GetDefense()
    {
        return defense;
    }

    public void SetAttack(int attack)
    {
        this.attack = attack;
    }

    public void SetDefense(int defense)
    {
        this.defense = defense;
    }

    public virtual void TakeDamage(int damageAmount, Unit fromUnit)
    {
        // value from -5 to 5, we add this value to damage
        Random rnd = new Random();
        int damageSpread = rnd.Next(-5, 6); 

        int realDamageAmount = damageAmount + damageSpread - defense;
        if (realDamageAmount < 1) realDamageAmount = 1; // min damage = 1
        currentHP -= realDamageAmount;
        if (currentHP < 0) currentHP = 0;
        UpdateHealthBar();

        if(blood!=null)
        {
            Instantiate(blood, transform.position, Quaternion.identity);
        }
        if(canvas!=null && floatingDamage!=null)
        {
            FloatingText fd = Instantiate(floatingDamage, transform.position, Quaternion.identity);
            fd.SetParentCanvas(canvas);
            fd.SetText(realDamageAmount.ToString());              
        }
    }

    public virtual void UpdateHealthBar() {}

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    public virtual List<Vector3Int> SetMovementPathTo(Vector3Int destinationCell)
    {
        Vector3Int currentCell = GetCell();
        if(currentCell.Equals(destinationCell))
        {
            movementPath = null;
            return null;
        }
        int offsetX = currentCell.x - pathfindingXMaxDistance;
        int offsetY = currentCell.y - pathfindingYMaxDistance;
        // We can't reach this cell anyway so we can just set movementPath to null
        if (Math.Abs(destinationCell.x - currentCell.x) > pathfindingXMaxDistance
            || Math.Abs(destinationCell.y - currentCell.y) > pathfindingYMaxDistance)
        {
            movementPath = null;
            return null;
        }
        List<Vector3Int> otherUnitsCells = gridManager.GetOccupiedCells();
        otherUnitsCells.Remove(this.GetCell());
        UnitsPathfinder pf = new UnitsPathfinder(offsetX, offsetY, pathfindingXMaxDistance * 2 + 1, pathfindingYMaxDistance * 2 + 1, gridManager.collidersTilemap, gridManager.groundTilemap, otherUnitsCells);
        List<Coords> movementPathCoords = pf.GetPath(currentCell.x - offsetX, currentCell.y - offsetY, destinationCell.x - offsetX, destinationCell.y - offsetY);
        if (movementPathCoords == null) return null;
        movementPath = movementPathCoords.ConvertAll<Vector3Int>(c => new Vector3Int(c.X, c.Y));
        return movementPath;
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

    public virtual void SetState(State newState)
    {
        state = newState;
        if (animator != null)
        {
            bool isWalking = state == State.IsMakingTurn && movementPath != null;
            if(isWalking)
                animator.SetBool("IsWalking", isWalking);
            else if(!isWalking)
                animator.SetBool("IsWalking", isWalking);
               
        }
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
        SetState(State.IsMakingTurn);
        // Skip turn. Unit has no action to do
        if (attackTarget == null && movementPath == null)
        {
            SetState(State.IsWaiting);
        }
    }

    public virtual void Die()
    {
        DropItem();
        if(animator!=null) animator.SetTrigger("DieTrigger");
        Destroy(gameObject, 1f); 
    }

    private void DropItem()
    {
        double chance = new Random().NextDouble() * 100;
        foreach (UnitData.Drop drop in unitData.drops)
        {
            if (chance < drop.dropChance)
            {
                // drop item
                ItemSpawner itemSpawner = GameObject.Find("Grid").GetComponent<ItemSpawner>();
                itemSpawner.SpawnItem(drop.itemData, gameObject.transform.position);
            }
        }
    }
}
