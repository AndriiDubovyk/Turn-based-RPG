using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemyGenerator : MonoBehaviour
{

    [SerializeField]
    private LevelGenerationData levelGenerationData;

    [SerializeField]
    private LevelGenerator levelGenerator;
    [SerializeField]
    private UnitSpawner unitSpawner;
    [SerializeField]
    private GameSaver gameSaver;

    void Awake()
    {
        if (gameSaver.IsSaveExist()) return;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("GameHandler").GetComponent<GameSaver>().IsSaveExist()) return;
        SpawnEnemiesInCommonRooms();
        SpawnEnemiesInBossRoom();
    }

    void SpawnEnemiesInBossRoom()
    {
        Random rnd = new Random();
        Vector3 roomPos = levelGenerator.GetBossRoomPos();
        int roomSize = levelGenerator.GetCellSize();
        List<Vector3> usedPositions = new List<Vector3>();
        
        foreach (LevelGenerationData.EnemyGenerationData egd in levelGenerationData.bossRoomEnemiesGenerationData)
        {
            if (GameProcessInfo.CurrentLevel >= egd.minLevel && GameProcessInfo.CurrentLevel <= egd.maxLevel)
            {
                int min = (int)(egd.minQuantity + egd.minQuantityIncreasePerLevel * (GameProcessInfo.CurrentLevel - 1));
                int max = (int)(egd.maxQuantity + egd.maxQuantityIncreasePerLevel * (GameProcessInfo.CurrentLevel - 1));

                for(int i = 0; i < max; i++)
                {
                    double chance = new Random().NextDouble() * 100;
                    
                    if(i < min || chance < egd.spawnChance)
                    {
                        Vector3 pos;
                        do
                        {
                            int x = rnd.Next(1, roomSize - 1);
                            int y = rnd.Next(1, roomSize - 1);
                            pos = roomPos + new Vector3(x, y);
                        } while (usedPositions.Contains(pos));
                        usedPositions.Add(pos);
                        unitSpawner.SpawnUnit(egd.enemyUnitData, pos);
                    }
                }
            }
        }

    }

    void SpawnEnemiesInCommonRooms()
    {
        Random rnd = new Random();
        List<Vector3> roomsPos = levelGenerator.GetCommonRoomPos();
        for(int j = 0; j< roomsPos.Count; j++)
        {
            Vector3 roomPos = roomsPos[j];
            int roomSize = levelGenerator.GetCellSize();
            List<Vector3> usedPositions = new List<Vector3>();


            foreach (LevelGenerationData.EnemyGenerationData egd in levelGenerationData.commonRoomEnemiesGenerationData)
            {
                if (GameProcessInfo.CurrentLevel >= egd.minLevel && GameProcessInfo.CurrentLevel <= egd.maxLevel)
                {
                    int min = (int)(egd.minQuantity + egd.minQuantityIncreasePerLevel * (GameProcessInfo.CurrentLevel - 1));
                    int max = (int)(egd.maxQuantity + egd.maxQuantityIncreasePerLevel * (GameProcessInfo.CurrentLevel - 1));

                    for (int i = 0; i < max; i++)
                    {
                        double chance = new Random().NextDouble() * 100;

                        if (i < min || chance < egd.spawnChance)
                        {
                            Vector3 pos;
                            do
                            {
                                int x = rnd.Next(1, roomSize - 1);
                                int y = rnd.Next(1, roomSize - 1);
                                pos = roomPos + new Vector3(x, y);
                            } while (usedPositions.Contains(pos));
                            usedPositions.Add(pos);
                            unitSpawner.SpawnUnit(egd.enemyUnitData, pos);
                        }
                    }
                }
            }

        }

    }
}
