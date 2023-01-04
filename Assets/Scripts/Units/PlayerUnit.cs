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

    private TextMeshProUGUI healthBar;

    private bool isPathConfirmed;
    private bool isItemTakingActive;

    private ItemData[] inventory = new ItemData[6];
    private ItemData equipedWeapon;

    private UI ui;


    protected override void Start()
    {
        base.Start();
        isPathConfirmed = false;
        isItemTakingActive = false;
        ui = GameObject.Find("UICanvas").GetComponent<UI>();
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<TextMeshProUGUI>();
    }

    public void SetAction()
    {
        GameObject invPanel = GameObject.Find("InventoryPanel");
        bool isInventoryOpened = invPanel != null && invPanel.activeSelf;

        if (Input.GetMouseButtonDown(0) && !isInventoryOpened && !ui.IsMouseOverUI())
        {
            Vector3 worldClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = gridManager.groundTilemap.WorldToCell(worldClickPos);
            Unit clickedUnit = gridManager.GetUnitAtCell(clickedCell);
            if (clickedUnit == null)
            {
                SelectDestinationCell(clickedCell);
                SetItemTaking(false);
            }
            else if (clickedUnit != gameObject && CanAttack(clickedUnit))
            {
                ChoseAttackTarget(clickedUnit);
            }
        }
    }

    public void TakeItem(ItemPickup itemPickup)
    {
        Debug.Log("Slots: " + NumberOfOccupiedInvetorySlots());
        if (HasFreeInventorySlots())
        {
            Debug.Log($"Player takes {itemPickup.itemData.name}");
            Destroy(itemPickup.gameObject);
            inventory[NumberOfOccupiedInvetorySlots()] = itemPickup.itemData;
        }
        SetItemTaking(false);
    }

    public void DropItem(ItemData itemData)
    {
        if(itemData ==equipedWeapon)
        {
            equipedWeapon = null;
            this.attack -= itemData.attack;
            return;
        }
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == itemData)
            {
                inventory[i] = null;
            }
            if (inventory[i] == null && i < inventory.Length - 1)
            {
                // shift all items
                inventory[i] = inventory[i + 1];
                inventory[i + 1] = null;
            }
        }
    }

    public ItemData[] GetInvetory()
    {
        return inventory;
    }

    public void EquipWeapon(ItemData newWeapon)
    {
        // Player can equip only item with damage or no item
        if (newWeapon == null || newWeapon.attack>0)
        {
            equipedWeapon = newWeapon;
            this.attack += equipedWeapon.attack;
        }
        Debug.Log("Equip " + equipedWeapon.name);
    }

    public ItemData GetEquipedWeapon()
    {
        return equipedWeapon;
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
