using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class PlayerUnit : Unit
{
    // Movement and taget selection overlay tile
    [SerializeField]
    private Tile selectionTile;
    [SerializeField]
    private Tile attackSelectionTile;
    [SerializeField]
    private Tile pathMarkTile;

    [SerializeField]
    private TextMeshProUGUI healthBar;

    private bool isPathConfirmed;
    private bool isItemTakingActive;

    private ItemData[] inventory = new ItemData[6];


    protected override void Start()
    {
        base.Start();
        isPathConfirmed = false;
        isItemTakingActive = false;
    }

    public void SetAction()
    {
        bool isInventoryOpened = GameObject.Find("InvetoryPanel") != null && GameObject.Find("InvetoryPanel").activeSelf;
        if (Input.GetMouseButtonDown(0) && !isInventoryOpened)
        {
            Vector3 worldClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = gridManager.groundTilemap.WorldToCell(worldClickPos);
            Unit clickedUnit = gridManager.GetUnitAtCell(clickedCell);
            if (clickedUnit == null)
            {
                SelectDestinationCell(clickedCell);
            }
            else if (clickedUnit != gameObject && CanAttack(clickedUnit))
            {
                ChoseAttackTarget(clickedUnit);
            }
        }
    }

    public void TakeItem(ItemPickup itemPickup)
    {
        Debug.Log("Slots: "+ NumberOfOccupiedInvetorySlots());
        if(HasFreeInventorySlots())
        {
            Debug.Log($"Player takes {itemPickup.itemData.name}");
            Destroy(itemPickup.gameObject);
            inventory[NumberOfOccupiedInvetorySlots()] = itemPickup.itemData;
        }
        SetItemTaking(false);
    }

    public ItemData[] GetInvetory()
    {
        return inventory;
    }

    private void SelectDestinationCell(Vector3Int clickedCell)
    {
        if (GetMovementPath() != null && clickedCell == GetMovementPath().Last() && !isPathConfirmed)
        {
            isPathConfirmed = true;
            ConfirmTurn();
        }
        else
        {
            isPathConfirmed = false;
            SetMovementPathTo(clickedCell);
            UpdateOverlayMarks();
        }
    }

    private void ChoseAttackTarget(Unit clickedUnit)
    {
        if (GetAttackTarget() == clickedUnit)
        {
            ConfirmTurn();
        }
        else
        {
            SetAttackTarget(clickedUnit);
            UpdateOverlayMarks();
        }
    }

    public bool IsPathConfirmed()
    {
        return isPathConfirmed;
    }

    public override void Attack(Unit another)
    {
        base.Attack(another);
        attackTarget = null;
        UpdateOverlayMarks();
    }

    public void SetItemTaking(bool active)
    {
        isItemTakingActive = active;
    }

    public bool IsItemTakingActive()
    {
        return isItemTakingActive;
    }

    public int NumberOfOccupiedInvetorySlots()
    {
        int slots = 0;
        for(int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null) slots++;
        }
        return slots;
    }

    public bool HasFreeInventorySlots()
    {
        return NumberOfOccupiedInvetorySlots() < inventory.Length;
    }

    public void UpdateOverlayMarks()
    {
        gridManager.uiOverlayTilemap.ClearAllTiles();

        Unit attackTarget = GetAttackTarget();

        if (attackTarget != null)
        {
            ShowAttackTargetSelectionOverlay(attackTarget);
        }
        else if (movementPath != null && movementPath.Count > 0)
        {
            ShowMovementPathOverlay(movementPath);
        }
    }

    public override void UpdateHealthBar()
    {
        healthBar.text = "HP: " + GetCurrentHP();
    }

    private void ShowAttackTargetSelectionOverlay(Unit attackTarget)
    {
        Vector3Int targetCell = attackTarget.GetCell();
        gridManager.uiOverlayTilemap.SetTile(targetCell, attackSelectionTile);
    }

    private void ShowMovementPathOverlay(List<Vector3Int> movementPath)
    {
        for (int i = 0; i < movementPath.Count; i++)
        {
            gridManager.uiOverlayTilemap.SetTile(movementPath[i], pathMarkTile);
        }
        gridManager.uiOverlayTilemap.SetTile(movementPath.Last(), selectionTile);
    }

    protected override void MoveUpdate()
    {
        base.MoveUpdate();
        UpdateOverlayMarks();
    }
}
