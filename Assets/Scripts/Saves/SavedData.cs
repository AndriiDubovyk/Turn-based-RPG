using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedData 
{
    
    [Serializable]
    public class LevelSavedData
    {
        public int[,] levelTemplete;
        public int levelIndex;
        public float exitPosX;
        public float exitPosY;
    }
    public LevelSavedData levelSavedData = new LevelSavedData();

    [Serializable]
    public class ItemSavedData
    {
        public String itemName;
        public float posX;
        public float posY;
    }
    public List<ItemSavedData> itemsSavedData = new List<ItemSavedData>();

    [Serializable]
    public class EnemySavedData
    {
        public string unitName;
        public float posX;
        public float posY;
        public int health;
    }
    public List<EnemySavedData> enemiesSavedData = new List<EnemySavedData>();

    [Serializable]
    public class PlayerSavedData
    {
        public float posX;
        public float posY;
        public int health;
        public int attack;
        public int defense;
        public string[] inventoryNames;
        public string equipedWeaponName;
        public string equipedArmorName;
    }
    public PlayerSavedData playerSavedData = new PlayerSavedData();


}
