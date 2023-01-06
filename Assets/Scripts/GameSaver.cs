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
    private const string SAVE_FILE_NAME = "SavedData.dat";

    private void Start()
    {
        Debug.Log($"Save exist {IsSaveExist()}");
        player = GameObject.Find("Player");
        if (IsSaveExist()) LoadGame();
        
    }

    private void OnApplicationQuit()
    {
        savedData = new SavedData();

        savedData.playerSavedData.posX = player.transform.position.x;
        savedData.playerSavedData.posY = player.transform.position.y;
        savedData.playerSavedData.health = player.GetComponent<PlayerUnit>().GetCurrentHP();
        savedData.playerSavedData.attack = player.GetComponent<PlayerUnit>().GetAttack();
        ItemData[] inv = player.GetComponent<PlayerUnit>().GetInventory();
        savedData.playerSavedData.inventoryNames = Array.ConvertAll(inv.ToArray(), it => it != null ? it.name : "");
        ItemData equipedWeapon = player.GetComponent<PlayerUnit>().GetEquipedWeapon();
        savedData.playerSavedData.equipedWeaponName = equipedWeapon!=null ? equipedWeapon.name : "";

        savedData.levelSavedData.levelIndex = GameProcessInfo.CurrentLevel;
        savedData.levelSavedData.levelTemplete = GameObject.Find("Grid").GetComponent<LevelGenerator>().GetLevelTemplete();

        List<GameObject> enemies = GameObject.Find("GameHandler").GetComponent<TurnManager>().GetEnemiesGO();
        foreach(GameObject enemy in enemies)
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
        Debug.Log("Game saved");
    }

    public bool IsSaveExist()
    {
        return File.Exists($"{Application.persistentDataPath}/{SAVE_FILE_NAME}");
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
            Debug.Log("Game loaded");
        }
    }

    private void GenerateWorld()
    {
        GameProcessInfo.CurrentLevel = savedData.levelSavedData.levelIndex;
        LevelGenerator lg = GameObject.Find("Grid").GetComponent<LevelGenerator>();
        lg.GenerateLevelWithTemplete(savedData.levelSavedData.levelTemplete);

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
        player.GetComponent<PlayerUnit>().SetHealth(savedData.playerSavedData.health);
        player.GetComponent<PlayerUnit>().SetAttack(savedData.playerSavedData.attack);
        player.GetComponent<PlayerUnit>().SetEquipedWeapon(savedData.playerSavedData.equipedWeaponName);
        player.GetComponent<PlayerUnit>().SetInventory(savedData.playerSavedData.inventoryNames);

    }
}
