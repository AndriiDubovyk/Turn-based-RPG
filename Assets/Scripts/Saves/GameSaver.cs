using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    private SavedData savedData;

    private GameObject player;
    private GameProcessInfo gpi;
    private const string SAVE_FILE_NAME = "SavedData.dat";

    private void Awake()
    {
        player = GameObject.Find("Player");
        GameObject[] objs = GameObject.FindGameObjectsWithTag("game_process_info");
        if (objs.Length > 0) gpi = objs[0].GetComponent<GameProcessInfo>();
        if (IsSaveExist())
        {
            LoadGame();
        }
        else
        {
            player.GetComponent<PlayerUnit>().InitUpgrades(); // upgrades work only with new game
        }
        
    }

    public void Save()
    {
        savedData = new SavedData();

        savedData.levelSavedData.levelIndex = gpi.CurrentDungeonLevel;
        savedData.levelSavedData.levelTemplete = GameObject.Find("Grid").GetComponent<LevelGenerator>().GetLevelTemplete();
        savedData.levelSavedData.exitPosX = GameObject.Find("Grid").GetComponent<LevelGenerator>().GetExitPosition().x;
        savedData.levelSavedData.exitPosY = GameObject.Find("Grid").GetComponent<LevelGenerator>().GetExitPosition().y;
        savedData.levelSavedData.revealedFogOfWarCells = GameObject.Find("Grid").GetComponent<GridManager>().GetRevealedFogOfWarCells();

        savedData.playerSavedData.posX = player.transform.position.x;
        savedData.playerSavedData.posY = player.transform.position.y;
        savedData.playerSavedData.health = player.GetComponent<PlayerUnit>().GetCurrentHP();
        savedData.playerSavedData.attack = player.GetComponent<PlayerUnit>().GetAttack();
        savedData.playerSavedData.defense = player.GetComponent<PlayerUnit>().GetDefense();
        ItemData[] inv = player.GetComponent<PlayerUnit>().GetInventory();
        savedData.playerSavedData.inventoryNames = Array.ConvertAll(inv.ToArray(), it => it != null ? it.name : "");
        ItemData equipedWeapon = player.GetComponent<PlayerUnit>().GetEquipedWeapon();
        savedData.playerSavedData.equipedWeaponName = equipedWeapon != null ? equipedWeapon.name : "";
        ItemData equipedArmor = player.GetComponent<PlayerUnit>().GetEquipedArmor();
        savedData.playerSavedData.equipedArmorName = equipedArmor != null ? equipedArmor.name : "";
        savedData.playerSavedData.level = player.GetComponent<PlayerUnit>().GetLevel();
        savedData.playerSavedData.exp = player.GetComponent<PlayerUnit>().GetExp();
        savedData.playerSavedData.maxHealth = player.GetComponent<PlayerUnit>().GetMaxHP();


        List<GameObject> enemies = GameObject.Find("GameHandler").GetComponent<TurnManager>().GetEnemiesGO();
        foreach (GameObject enemy in enemies)
        {
            SavedData.EnemySavedData enemySavedData = new SavedData.EnemySavedData();
            enemySavedData.unitName = enemy.GetComponent<Unit>().unitData.name;
            enemySavedData.posX = enemy.transform.position.x;
            enemySavedData.posY = enemy.transform.position.y;
            enemySavedData.health = enemy.GetComponent<Unit>().GetCurrentHP();
            savedData.enemiesSavedData.Add(enemySavedData);
        }

        List<ItemPickup> itemPickups = GameObject.Find("Grid").GetComponent<GridManager>().GetItemPickupList();
        foreach (ItemPickup itemPickup in itemPickups)
        {
            SavedData.ItemSavedData itemSavedData = new SavedData.ItemSavedData();
            itemSavedData.itemName = itemPickup.itemData.name;
            itemSavedData.posX = itemPickup.gameObject.transform.position.x;
            itemSavedData.posY = itemPickup.gameObject.transform.position.y;
            savedData.itemsSavedData.Add(itemSavedData);
        }

        SaveGame();
    }

    private void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create($"{Application.persistentDataPath}/{SAVE_FILE_NAME}");
        bf.Serialize(file, savedData);
        file.Close();
    }

    public bool IsSaveExist()
    {
        return File.Exists($"{Application.persistentDataPath}/{SAVE_FILE_NAME}");
    }

    public void DeleteSave()
    {
        if(IsSaveExist())
        {
            File.Delete($"{Application.persistentDataPath}/{SAVE_FILE_NAME}");
        }
    }

    public void LoadGame()
    {
        if (IsSaveExist())
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open($"{Application.persistentDataPath}/{SAVE_FILE_NAME}", FileMode.Open);
            savedData = (SavedData)bf.Deserialize(file);
            file.Close();
            GenerateWorld();
        }
    }

    private void GenerateWorld()
    {
        gpi.CurrentDungeonLevel = savedData.levelSavedData.levelIndex;
        LevelGenerator lg = GameObject.Find("Grid").GetComponent<LevelGenerator>();
        lg.GenerateLevelWithTemplete(savedData.levelSavedData.levelTemplete, new Vector3(savedData.levelSavedData.exitPosX, savedData.levelSavedData.exitPosY));
        GameObject.Find("Grid").GetComponent<GridManager>().RevealFogOfWar(savedData.levelSavedData.revealedFogOfWarCells);

        ItemSpawner itemSpawner = GameObject.Find("Grid").GetComponent<ItemSpawner>();
        foreach (SavedData.ItemSavedData isd in savedData.itemsSavedData)
        {
            itemSpawner.SpawnItem(isd.itemName, new Vector3(isd.posX, isd.posY));
        }

        UnitSpawner unitSpanwer = GameObject.Find("Grid").GetComponent<UnitSpawner>();
        foreach (SavedData.EnemySavedData esd in savedData.enemiesSavedData)
        {
            unitSpanwer.SpawnUnit(esd.unitName, new Vector3(esd.posX, esd.posY), esd.health);
        }

        player.transform.position = new Vector3(savedData.playerSavedData.posX, savedData.playerSavedData.posY);
        player.GetComponent<PlayerUnit>().SetMaxHealth(savedData.playerSavedData.maxHealth);
        player.GetComponent<PlayerUnit>().SetHealth(savedData.playerSavedData.health);
        player.GetComponent<PlayerUnit>().SetAttack(savedData.playerSavedData.attack);
        player.GetComponent<PlayerUnit>().SetDefense(savedData.playerSavedData.defense);
        player.GetComponent<PlayerUnit>().SetEquipedWeapon(savedData.playerSavedData.equipedWeaponName);
        player.GetComponent<PlayerUnit>().SetEquipedArmor(savedData.playerSavedData.equipedArmorName);
        player.GetComponent<PlayerUnit>().SetInventory(savedData.playerSavedData.inventoryNames);
        player.GetComponent<PlayerUnit>().SetLevel(savedData.playerSavedData.level);
        player.GetComponent<PlayerUnit>().SetExp(savedData.playerSavedData.exp);

    }
}
