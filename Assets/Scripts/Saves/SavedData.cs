using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedData 
{

    [Serializable]
    public struct Cell
    {
        public int x;
        public int y;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector3Int(Cell rValue)
        {
            return new Vector3Int(rValue.x, rValue.y, 0);
        }

        public static implicit operator Cell(Vector3Int rValue)
        {
            return new Cell(rValue.x, rValue.y);
        }
    }

    [Serializable]
    public struct LevelSavedData
    {
        public int[,] levelTemplete;
        public int levelIndex;
        public float exitPosX;
        public float exitPosY;
        public List<Cell> revealedFogOfWarCells;
    }
    public LevelSavedData levelSavedData = new LevelSavedData();

    [Serializable]
    public struct ItemSavedData
    {
        public String itemName;
        public float posX;
        public float posY;
    }
    public List<ItemSavedData> itemsSavedData = new List<ItemSavedData>();

    [Serializable]
    public struct EnemySavedData
    {
        public string unitName;
        public float posX;
        public float posY;
        public int health;
    }
    public List<EnemySavedData> enemiesSavedData = new List<EnemySavedData>();

    [Serializable]
    public struct PlayerSavedData
    {
        public float posX;
        public float posY;
        public int maxHealth;
        public int health;
        public int attack;
        public int defense;
        public int level;
        public int exp;
        public string[] inventoryNames;
        public string equipedWeaponName;
        public string equipedArmorName;
    }
    public PlayerSavedData playerSavedData = new PlayerSavedData();


}
