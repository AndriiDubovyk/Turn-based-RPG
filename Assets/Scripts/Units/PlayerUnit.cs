using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using Random = System.Random;

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

    private ItemData[] inventory = new ItemData[6];
    private ItemData equipedWeapon;
    private ItemData equipedArmor;


    [SerializeField]
    private ItemsPickupList ips;

    private UI ui;
    private TurnManager tm;
    private ResultPanel resultPanel;
    [SerializeField]
    private FloatingText floatingWaitText;
    [SerializeField]
    private FloatingText floatingInfoText;

    private Vector3Int lastCellWalkAudioWasPlaying;
    private Vector3 mouseDownPos;
    private List<Vector3Int> mouseDownPrecalculatedPath;
    private GameProcessInfo gpi;

    [SerializeField]
    private ItemData healingPotionItemData;
    private int librarianLevel = 0;
    private int alchemistLevel = 0;
    


    protected override void Awake()
    {
        base.Awake();

        GameObject[] objs = GameObject.FindGameObjectsWithTag("game_process_info");
        if (objs.Length > 0) gpi = objs[0].GetComponent<GameProcessInfo>();

        tm = GameObject.Find("GameHandler").GetComponent<TurnManager>();
        tm.AddPlayer(gameObject);
        resultPanel = GameObject.Find("ResultPanel").GetComponent<ResultPanel>();
        lastCellWalkAudioWasPlaying = new Vector3Int(-1, -1, -1);
        level = 1;
        exp = 0;
    }


    public void InitUpgrades()
    {
        librarianLevel = villageInfo.GetLibrarianLevel();
        alchemistLevel = villageInfo.GetAlchemistLevel();
        for(int i = 0; i < villageInfo.villageUpgradesData.alchemistUpgrades.Count; i++)
        {
            
            VillageUpgradesData.AlchemistUpgrade upgrade = villageInfo.villageUpgradesData.alchemistUpgrades[i];
            if (villageInfo.GetAlchemistLevel() > i)
            {
                for(int j =0; j<upgrade.plusStartPotion; j++)
                {
                    inventory[NumberOfOccupiedInvetorySlots()] = healingPotionItemData.Clone();
                }
            }
        }
        for (int i = 0; i < villageInfo.villageUpgradesData.smithUpgrades.Count; i++)
        {
           VillageUpgradesData.SmithUpgrade upgrade = villageInfo.villageUpgradesData.smithUpgrades[i];
           if (villageInfo.GetSmithLevel()>i)
           {
                if (upgrade.startWeapon != null) equipedWeapon = upgrade.startWeapon;
                if (upgrade.startArmor != null) equipedArmor = upgrade.startArmor;
                attack += upgrade.plusWeaponAttack;
                defense += upgrade.plusArmorDefense;
           }
        }
    }


    protected override void Start()
    {
        base.Start();
        isPathConfirmed = false;
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

        if (gpi.CurrentDungeonLevel > 1 && !isSaveExist) LoadData();
        UpdateHealthBar();
        playerLevelInfo.UpdateLevelInfo(level, exp, levelingData.GetExpToNextLevel(level + 1));
        GameObject.Find("MainCamera").GetComponent<MainCamera>().CenterOnPlayer();
        gridManager.UpdateVisibility();
    }

    public void SaveData()
    {
        gpi.CurrentHP = currentHP;
        gpi.MaxHP = maxHP;
        gpi.Attack = attack;
        gpi.Defense = defense;
        gpi.Level = level;
        gpi.Exp = exp;
        gpi.Inventory = inventory;
        gpi.EquipedWeapon = equipedWeapon;
        gpi.EquipedArmor = equipedArmor;
    }

    public void LoadData()
    {
        currentHP = gpi.CurrentHP;
        maxHP = gpi.MaxHP;
        attack = gpi.Attack;
        defense = gpi.Defense;
        inventory = gpi.Inventory;
        level = gpi.Level;
        exp = gpi.Exp;
        equipedWeapon = gpi.EquipedWeapon;
        equipedArmor = gpi.EquipedArmor;
        playerLevelInfo.UpdateLevelInfo(level, exp, levelingData.GetExpToNextLevel(level - 1));

    }


    public int GetLevel()
    {
        return level;
    }

    public void SetLevel(int level)
    {
        this.level = level;
        playerLevelInfo.UpdateLevelInfo(level, exp, levelingData.GetExpToNextLevel(level + 1));
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
        for (int i = 0; i < inventory.Length; i++)
        {
            foreach (ItemPickup ip in ips.itemsPickup)
            {
                if (ip.itemData.name == inventory[i])
                {
                    inv[i] = ip.itemData;
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
        foreach (ItemPickup ip in ips.itemsPickup)
        {
            if (ip.itemData.name == equipedWeaponName)
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
        if (ui.IsUIBlockingActions()) return;
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
            Vector3 worldClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = gridManager.groundTilemap.WorldToCell(worldClickPos);
            Unit clickedUnit = gridManager.GetUnitAtCell(clickedCell);
            if (clickedUnit == null)
            {
                List<Vector3Int> tmp = movementPath != null ? new List<Vector3Int>(movementPath) : null;
                movementPath = SetMovementPathTo(clickedCell);
                if (movementPath != null) mouseDownPrecalculatedPath = new List<Vector3Int>(movementPath);
                movementPath = tmp;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mouseUpPos = Input.mousePosition;
            if (Vector3.Distance(mouseDownPos, mouseUpPos) > 100f) return; // panning

            Vector3 worldClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = gridManager.groundTilemap.WorldToCell(worldClickPos);
            if (gridManager.fogOfWarTilemap.GetTile(clickedCell) != null) return;
            Unit clickedUnit = gridManager.GetUnitAtCell(clickedCell);
            if (clickedUnit == null)
            {
                attackTarget = null;
                SelectDestinationCell(clickedCell);
                mouseDownPrecalculatedPath = null;
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
        if (state != State.IsThinking) return; // Can take item only on own turn
        if (HasFreeInventorySlots())
        {
            SkipTurn();
            gridManager.RemoveItemPickup(itemPickup);
            Destroy(itemPickup.gameObject);
            AddItem(itemPickup.itemData);
            if (audioManager != null) audioManager.PlayTakeItemSound();
            GameObject.Find("UICanvas").GetComponent<UI>().UpdateItemTakeScrollItems();
        } else
        {
            ShowInfo("My bag is full");
        }
        ResetMovementPath();
        // Exit from item take scroll to be able move again
        ui.EnterItemTakeScroll(false);
    }

    public void AddItem(ItemData itemData)
    {
        if (HasFreeInventorySlots())
        {
            inventory[NumberOfOccupiedInvetorySlots()] = itemData;

            GameObject invPanel = GameObject.Find("InventoryPanel");
            if (invPanel != null && invPanel.GetComponent<InventoryPanel>() != null)
            {
                invPanel.GetComponent<InventoryPanel>().InitializeInventory();
            }
        }
    }

    public void Heal(int healing)
    {
        int realHealing = healing;
        for (int i = 0; i < villageInfo.villageUpgradesData.alchemistUpgrades.Count; i++)
        {
            VillageUpgradesData.AlchemistUpgrade upgrade = villageInfo.villageUpgradesData.alchemistUpgrades[i];
            if (alchemistLevel > i)
            {
                realHealing += upgrade.plusPotionPower;
            }
        }


        currentHP += realHealing;
        if (currentHP > maxHP) currentHP = maxHP;
        UpdateHealthBar();
    }


    public void SetMaxHealth(int maxHealth)
    {
        this.maxHP = maxHealth;
        UpdateHealthBar();
    }

    public void RemoveItem(ItemData itemData)
    {
        if (itemData == equipedWeapon)
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
        if (newWeapon == null || newWeapon.attack > 0)
        {
            if (equipedWeapon == null)
            {
                equipedWeapon = newWeapon;
                if (equipedWeapon != null) this.attack += equipedWeapon.attack;
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
    }

    public void SkipTurn()
    {
        if (state == State.IsThinking)
            SetState(State.IsWaiting);
    }

    public override void SetState(State newState)
    {
        base.SetState(newState);
        bool isWalking = state == State.IsMakingTurn && movementPath != null && movementPath.Count > 0;
        bool wasAudioPlayingAtThisCell = movementPath != null ? (lastCellWalkAudioWasPlaying == movementPath[0]) : true;
        if (isWalking && !wasAudioPlayingAtThisCell)
        {
            if (movementPath != null) lastCellWalkAudioWasPlaying = movementPath[0];
            if (audioManager != null) audioManager.PlayWalkSound();
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
            if (mouseDownPrecalculatedPath != null)
            {
                movementPath = new List<Vector3Int>(mouseDownPrecalculatedPath);
                mouseDownPrecalculatedPath = null;
            }
            else
            {
                SetMovementPathTo(clickedCell);
            }
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
        // Flip sprite if requires   
        FlipToPoint(another.transform.position);
        if (animator != null) animator.SetTrigger("AttackTrigger");

        int attackValue = this.attack;
        for(int i=0; i < villageInfo.villageUpgradesData.librarianUpgrades.Count; i++)
        {
            VillageUpgradesData.LibrarianUpgrade upgrade = villageInfo.villageUpgradesData.librarianUpgrades[i];
            if (librarianLevel > i)
            {
                if (upgrade.againstEnemy.name == another.unitData.name)
                {
                    attackValue += upgrade.attackBonus;
                }
            }
        }
        another.TakeDamage(attackValue, this);

        attackTarget = null;
        UpdateOverlayMarks();
        if (audioManager != null) audioManager.PlayAttackSound();
    }

    public override void TakeDamage(int damageAmount, Unit fromUnit)
    {
        int realDefense = defense;
        for (int i = 0; i < villageInfo.villageUpgradesData.librarianUpgrades.Count; i++)
        {
            VillageUpgradesData.LibrarianUpgrade upgrade = villageInfo.villageUpgradesData.librarianUpgrades[i];
            if (librarianLevel > i)
            {
                if (upgrade.againstEnemy.name == fromUnit.unitData.name)
                {
                    realDefense += upgrade.defenseBonus;
                }
            }
        }
        // value from -5 to 5, we add this value to damage
        Random rnd = new Random();
        int damageSpread = rnd.Next(-5, 6);

        int realDamageAmount = damageAmount + damageSpread - realDefense;

        if (realDamageAmount < 1) realDamageAmount = 1; // min damage = 1
        currentHP -= realDamageAmount;
        if (currentHP < 0) currentHP = 0;
        UpdateHealthBar();

        if (blood != null)
        {
            Instantiate(blood, transform.position, Quaternion.identity);
        }
        if (canvas != null && floatingDamage != null)
        {
            FloatingText fd = Instantiate(floatingDamage, transform.position, Quaternion.identity);
            fd.SetParentCanvas(canvas);
            fd.SetText(realDamageAmount.ToString());
        }
    }


    public int NumberOfOccupiedInvetorySlots()
    {
        int slots = 0;
        for (int i = 0; i < inventory.Length; i++)
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

    public void ResetMovementPath()
    {
        SetMovementPathTo(GetCell());
        UpdateOverlayMarks();
        SetState(Unit.State.IsThinking);
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

    public void ShowFloatingWaitText()
    {
        if (canvas != null && floatingWaitText != null)
        {
            FloatingText fd = Instantiate(floatingWaitText, transform.position, Quaternion.identity);
            fd.SetParentCanvas(canvas);
        }
    }

    public void ShowInfo(string str)
    {
        if (canvas != null && floatingInfoText != null)
        {
            FloatingText fi = Instantiate(floatingInfoText, transform.position, Quaternion.identity);
            fi.SetText(str);
            fi.SetParentCanvas(canvas);
        }
    }
}
