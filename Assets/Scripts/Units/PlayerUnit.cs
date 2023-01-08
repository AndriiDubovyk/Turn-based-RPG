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
    private ItemData equipedArmor;

    private UI ui;
    private TurnManager tm;
    private ResultPanel resultPanel;
    [SerializeField]
    private ItemsPickupList ips;

    protected override void Awake()
    {
        base.Awake();
        tm = GameObject.Find("GameHandler").GetComponent<TurnManager>();
        tm.AddPlayer(gameObject);
        resultPanel = GameObject.Find("ResultPanel").GetComponent<ResultPanel>();
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<TextMeshProUGUI>();
    }

    public void SaveData()
    {
        GameProcessInfo.CurrentHP = currentHP;
        GameProcessInfo.Attack = attack;
        GameProcessInfo.Inventory = inventory;
        GameProcessInfo.EquipedWeapon = equipedWeapon;
    }

    public void SetInventory(ItemData[] inventory)
    {
        this.inventory = inventory;
    }

    public void SetInventory(string[] inventory)
    {
        ItemData[] inv = new ItemData[6];
        for(int i=0; i<inventory.Length; i++)
        {
            foreach (ItemPickup ip in ips.itemsPickup)
            {
                if (ip.itemData.name == inventory[i])
                {
                    inv [i]= ip.itemData;
                }
            }
        }

        this.inventory = inv;
    }

    public void SetEquipedWeapon(ItemData equipedWeapon)
    {
        this.equipedWeapon = equipedWeapon;
    }

    public void SetEquipedArmor(ItemData equipedArmor)
    {
        this.equipedArmor = equipedArmor;
    }

    public void SetEquipedWeapon(string equipedWeaponName)
    {
        ItemData ew = null;
        foreach(ItemPickup ip in ips.itemsPickup)
        {
            if(ip.itemData.name == equipedWeaponName)
            {
                ew = ip.itemData;
            }
        }
        this.equipedWeapon = ew;
    }

    public void SetEquipedArmor(string equipedArmorName)
    {
        ItemData ea = null;
        foreach (ItemPickup ip in ips.itemsPickup)
        {
            if (ip.itemData.name == equipedArmorName)
            {
                ea = ip.itemData;
            }
        }
        this.equipedArmor = ea;
    }

    public void LoadData()
    {
        currentHP = GameProcessInfo.CurrentHP;
        attack = GameProcessInfo.Attack;
        inventory = GameProcessInfo.Inventory;
        equipedWeapon = GameProcessInfo.EquipedWeapon;
        equipedArmor = GameProcessInfo.EquipedArmor;
    }

    protected override void Start()
    {
        base.Start();
        isPathConfirmed = false;
        isItemTakingActive = false;
        ui = GameObject.Find("UICanvas").GetComponent<UI>();

        LevelGenerator lg = GameObject.Find("Grid").GetComponent<LevelGenerator>();
        bool isSaveExist = GameObject.Find("GameHandler").GetComponent<GameSaver>().IsSaveExist();
        if (lg!=null && !isSaveExist)
        {
            // player starts at the center of the room
            int cellSize = lg.GetCellSize();
            Vector3 startRoomPos = lg.GetStartRoomPos();
            gameObject.transform.position = new Vector3(startRoomPos.x + cellSize / 2, startRoomPos.y + cellSize / 2);
        }

        if (GameProcessInfo.CurrentLevel > 1 && !isSaveExist) LoadData();
        UpdateHealthBar();
    }

    protected override void Update()
    {
        base.Update();
        if (IsDead()) Die();
    }

    public void SetAction()
    {
        if (Input.GetMouseButtonDown(0) && !ui.IsUIBlockingActions())
        {
            Vector3 worldClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = gridManager.groundTilemap.WorldToCell(worldClickPos);
            Unit clickedUnit = gridManager.GetUnitAtCell(clickedCell);
            if (clickedUnit == null)
            {
                attackTarget = null;
                SelectDestinationCell(clickedCell);
                SetItemTaking(false);
            }
            else if (clickedUnit != this && CanAttack(clickedUnit))
            {
                ChoseAttackTarget(clickedUnit);
                movementPath = null;
            }
        }
    }

    public void TakeItem(ItemPickup itemPickup)
    {
        if (HasFreeInventorySlots())
        {
            Destroy(itemPickup.gameObject);
            AddItem(itemPickup.itemData);
        }
        SetItemTaking(false);
    }

    public void AddItem(ItemData itemData)
    {
        if (HasFreeInventorySlots())
        {   
            inventory[NumberOfOccupiedInvetorySlots()] = itemData;

            GameObject invPanel = GameObject.Find("InventoryPanel");
            if(invPanel!=null && invPanel.GetComponent<InventoryPanel>()!=null)
            {
                invPanel.GetComponent<InventoryPanel>().InitializeInventory();
            }
        }
    }

    public void Heal(int healing)
    {
        currentHP += healing;
        if (currentHP > maxHP) currentHP = maxHP;
        UpdateHealthBar();
        SkipTurn();
    }

    public void RemoveItem(ItemData itemData)
    {
        if(itemData ==equipedWeapon)
        {
            equipedWeapon = null;
            this.attack -= itemData.attack;
            return;
        }
        if (itemData == equipedArmor)
        {
            equipedArmor = null;
            this.defense -= itemData.defense;
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

    public ItemData[] GetInventory()
    {
        return inventory;
    }

    public void EquipWeapon(ItemData newWeapon)
    {
        // Player can equip only item with damage or no item
        if (newWeapon == null || newWeapon.attack>0)
        {
            if(equipedWeapon == null)
            {
                equipedWeapon = newWeapon;
                if(equipedWeapon!=null) this.attack += equipedWeapon.attack;
            }
            else
            {
                ItemData tmpItem = equipedWeapon;
                RemoveItem(tmpItem); // alread has defense decreasing
                equipedWeapon = newWeapon;
                AddItem(tmpItem);
               
                if (equipedWeapon != null) this.attack += equipedWeapon.attack;
            }
        }
        SkipTurn();
    }

    public void EquipArmor(ItemData newArmor)
    {
        // Player can equip only item with defense or no item
        if (newArmor == null || newArmor.defense > 0)
        {
            if (equipedArmor == null)
            {
                equipedArmor = newArmor;
                if (equipedArmor != null) this.defense += equipedArmor.defense;
            }
            else
            {
                ItemData tmpItem = equipedArmor;
                RemoveItem(tmpItem); // already has defense decreasing
                equipedArmor = newArmor;
                AddItem(tmpItem);

                if (equipedArmor != null) this.defense += equipedArmor.defense;
            }
        }
        SkipTurn();
    }

    public void SkipTurn()
    {
        if (state == State.IsThinking)
            SetState(State.IsWaiting);
    }

    public ItemData GetEquipedWeapon()
    {
        return equipedWeapon;
    }

    public ItemData GetEquipedArmor()
    {
        return equipedArmor;
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

    public override void Die()
    {
        resultPanel.SetResult(false);
        resultPanel.Display();
    }

    protected override void MoveUpdate()
    {
        base.MoveUpdate();
        UpdateOverlayMarks();
    }
}
