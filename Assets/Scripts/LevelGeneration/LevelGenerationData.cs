using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewLevelGenerationData", menuName = "SO/LevelGenerationData")]
public class LevelGenerationData : ScriptableObject
{

    public float startCommonRoomsNumber = 2;
    public float commonRoomsNumberIncreasePerLevel;
    
    [Serializable]
    public class EnemyGenerationData
    {
        public UnitData enemyUnitData;
        public float minQuantity;
        public float maxQuantity;
        public float minQuantityIncreasePerLevel;
        public float maxQuantityIncreasePerLevel;
        public int minLevel;
        public int maxLevel;
        public float spawnChance; // chanse of spawn of every entity after minQuantity
    }
    public List<EnemyGenerationData> commonRoomEnemiesGenerationData;
    public List<EnemyGenerationData> bossRoomEnemiesGenerationData;

    [Serializable]
    public class ItemGenerationData
    {
        public ItemData itemData;
        public float minQuantity;
        public float maxQuantity;
        public float minQuantityIncreasePerLevel;
        public float maxQuantityIncreasePerLevel;
        public int minLevel;
        public int maxLevel;
        public float spawnChance; // chanse of spawn of every entity after minQuantity
    }
    public List<ItemGenerationData> startRoomItemGenerationData;
    public List<ItemGenerationData> commonRoomItemGenerationData;
    public List<ItemGenerationData> bossRoomItemGenerationData;


}

