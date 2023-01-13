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
    private PlayerHealthBar healthBar;
    [SerializeField]
    private PlayerLevelInfo playerLevelInfo;

    public LevelingData levelingData;
    private int level;
    private int exp;

    private bool isPathConfirmed;
    private bool isItemTakingActive;

    private ItemData[] inventory = new ItemData[6];
    private ItemData equipedWeapon;
    private ItemData equipedArmor;


    [SerializeField]
    private ItemsPickupList ips;

    private UI ui;
    private TurnManager tm;
    private ResultPanel resultPanel;
    [SerializeField]
    private AudioSource attackAudio;

    [SerializeField]
    private AudioSource walkAudio;
    private Vector3Int lastCellWalkAudioWasPlaying;
    

    protected override void Awake()
    {
        base.Awake();
        tm = GameObject.Find("GameHandler").GetComponent<TurnManager>();
        tm.AddPlayer(gameObject);
        resultPanel = GameObject.Find("ResultPanel").GetComponent<ResultPanel>();
        lastCellWalkAudioWasPlaying = new Vector3Int(-1, -1, -1);
        level = 1;
        exp = 0;
    }


    protected override void Start()
    {
        base.Start();
        isPathConfirmed = false;
        isItemTakingActive = false;
        ui = GameObject.Find("UICanvas").GetComponent<UI>();

        LevelGenerator lg = GameObject.Find("Grid").GetComponent<LevelGenerator>();
        bool isSaveExist = GameObject.Find("GameHandler").GetComponent<GameSaver>().IsSaveExist();
        if (lg != null && !isSaveExist)
        {
            // player starts at the center of the room
            int cellSize = lg.GetCellSize();
            Vector3 startRoomPos = lg.GetStartRoomPos();
            gameObject.transform.position = new Vector3(startRoomPos.x + cellSize / 2, startRoomPos.y + cellSize / 2);
        }

        if (GameProcessInfo.CurrentDungeonLevel > 1 && !isSaveExist) LoadData();
        UpdateHealthBar();
        playerLevelInfo.UpdateLevelInfo(level, exp, levelingData.GetExpToNextLevel(level + 1));
    }

    public void SaveData()
    {  
        GameProcessInfo.CurrentHP = currentHP;
        GameProcessInfo.MaxHP = maxHP;
        GameProcessInfo.Attack = attack;
        GameProcessInfo.Defense = defense;
        GameProcessInfo.Level = level;
        GameProcessInfo.Exp = exp;
        GameProcessInfo.Inventory = inventory;
        GameProcessInfo.EquipedWeapon = equipedWeapon;
    }

    public void LoadData()
    {
        currentHP = GameProcessInfo.CurrentHP;
        maxHP = GameProcessInfo.MaxHP;
        attack = GameProcessInfo.Attack;
        defense = GameProcessInfo.Defense;
        inventory = GameProcessInfo.Inventory;
        level = GameProcessInfo.Level;
        exp = GameProcessInfo.Exp;
        equipedWeapon = GameProcessInfo.EquipedWeapon;
        equipedArmor = GameProcessInfo.EquipedArmor;
        playerLevelInfo.UpdateLevelInfo(level, exp, levelingData.GetExpToNextLevel(level - 1));

    }


    public int GetLevel()
    {
        return level;
    }

    public void SetLevel(int level)
    {
        this.level = level;
        playerLevelInfo.UpdateLevelInfo(level, exp, levelingData.GetExpToNextLevel(level+1));
    }

    public int GetExp()
    {
        return exp;
    }

    public void SetExp(int exp)
    {
        this.exp = exp;
        playerLevelInfo.UpdateLevelInfo(level, exp, levelingData.GetExpToNextLevel(level + 1));
    }

    public void AddExp(int plusExp)
    {
        this.exp += plusExp;
        while (this.exp >= levelingData.GetExpToNextLevel(level + 1))
        {
            this.exp -= levelingData.GetExpToNextLevel(level + 1);
            GetLevelUp();   
        }
        playerLevelInfo.UpdateLevelInfo(level, exp, levelingData.GetExpToNextLevel(level + 1));
    }

    private void GetLevelUp()
    {
        this.level += 1;
        this.maxHP += levelingData.maxHealthPerLevel;
        this.currentHP += levelingData.maxHealthPerLevel;
        this.attack += levelingData.attackPerLevel;
        this.defense += levelingData.defensePerLevel;
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


    public void SetMaxHealth(int maxHealth)
    {
        this.maxHP = maxHealth;
        UpdateHealthBar();
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

    public override void SetState(State newState)
    {
        base.SetState(newState);
        bool isWalking = state == State.IsMakingTurn && movementPath != null && movementPath.Count>0;
        bool wasAudioPlayingAtThisCell = movementPath != null ? (lastCellWalkAudioWasPlaying == movementPath[0]) : true;
        if (isWalking && !wasAudioPlayingAtThisCell)
        {
            if(movementPath!=null) lastCellWalkAudioWasPlaying = movementPath[0];
            walkAudio.Play();
        }
            
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
        attackAudio.Play();
    }

    public void SetItemTaking(bool active)
    {
        if (active && GetState() != State.IsThinking) return; //  // Possible only on player's turn
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
        healthBar.UpdateHealth(GetCurrentHP(), GetMaxHP());
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
