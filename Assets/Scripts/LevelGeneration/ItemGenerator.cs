using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ItemGenerator : MonoBehaviour
{

    [SerializeField]
    private LevelGenerationData levelGenerationData;

    [SerializeField]
    private LevelGenerator levelGenerator;
    [SerializeField]
    private ItemSpawner itemSpawner;
    private GameProcessInfo gpi;


    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("game_process_info");
        if (objs.Length > 0) gpi = objs[0].GetComponent<GameProcessInfo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("GameHandler").GetComponent<GameSaver>().IsSaveExist()) return;
        levelGenerator = gameObject.GetComponent<LevelGenerator>();
        itemSpawner = gameObject.GetComponent<ItemSpawner>();
        SpawnItemsInStartRoom();
        SpawnItemsInCommonRooms();
        SpawnItemsInBossRoom();
    }

    private void SpawnItemsInStartRoom()
    {
        Random rnd = new Random();
        Vector3 roomPos = levelGenerator.GetStartRoomPos();
        int roomSize = levelGenerator.GetCellSize();
        List<Vector3> usedPositions = new List<Vector3>();

        foreach (LevelGenerationData.ItemGenerationData
            igd in levelGenerationData.startRoomItemGenerationData)
        {
            if (gpi.CurrentDungeonLevel >= igd.minLevel && gpi.CurrentDungeonLevel <= igd.maxLevel)
            {
                int min = (int)(igd.minQuantity + igd.minQuantityIncreasePerLevel * (gpi.CurrentDungeonLevel - 1));
                int max = (int)(igd.maxQuantity + igd.maxQuantityIncreasePerLevel * (gpi.CurrentDungeonLevel - 1));

                for (int i = 0; i < max; i++)
                {
                    double chance = new Random().NextDouble() * 100;

                    if (i < min || chance < igd.spawnChance)
                    {
                        Vector3 pos;
                        do
                        {
                            int x = rnd.Next(1, roomSize - 1);
                            int y = rnd.Next(1, roomSize - 1);
                            pos = roomPos + new Vector3(x, y);
                        } while (usedPositions.Contains(pos));
                        usedPositions.Add(pos);
                        itemSpawner.SpawnItem(igd.itemData, pos);
                    }
                }
            }
        }
    }

    private void SpawnItemsInCommonRooms()
    {
        Random rnd = new Random();
        List<Vector3> roomsPos = levelGenerator.GetCommonRoomPos();
        int roomSize = levelGenerator.GetCellSize();
        for (int j=0; j< levelGenerator.GetCommonRoomPos().Count; j++)
        {
            Vector3 roomPos = levelGenerator.GetCommonRoomPos()[j];
            List<Vector3> usedPositions = new List<Vector3>();
            foreach (LevelGenerationData.ItemGenerationData
            igd in levelGenerationData.commonRoomItemGenerationData)
            {
                if (gpi.CurrentDungeonLevel >= igd.minLevel && gpi.CurrentDungeonLevel <= igd.maxLevel)
                {
                    int min = (int)(igd.minQuantity + igd.minQuantityIncreasePerLevel * (gpi.CurrentDungeonLevel - 1));
                    int max = (int)(igd.maxQuantity + igd.maxQuantityIncreasePerLevel * (gpi.CurrentDungeonLevel - 1));

                    for (int i = 0; i < max; i++)
                    {
                        double chance = new Random().NextDouble() * 100;

                        if (i < min || chance < igd.spawnChance)
                        {
                            Vector3 pos;
                            do
                            {
                                int x = rnd.Next(1, roomSize - 1);
                                int y = rnd.Next(1, roomSize - 1);
                                pos = roomPos + new Vector3(x, y);
                            } while (usedPositions.Contains(pos));
                            usedPositions.Add(pos);
                            itemSpawner.SpawnItem(igd.itemData, pos);
                        }
                    }
                }
            }
        }
    }

    private void SpawnItemsInBossRoom()
    {
        Random rnd = new Random();
        Vector3 roomPos = levelGenerator.GetBossRoomPos();
        int roomSize = levelGenerator.GetCellSize();
        List<Vector3> usedPositions = new List<Vector3>();
        foreach (LevelGenerationData.ItemGenerationData
            igd in levelGenerationData.bossRoomItemGenerationData)
        {
            if (gpi.CurrentDungeonLevel >= igd.minLevel && gpi.CurrentDungeonLevel <= igd.maxLevel)
            {
                int min = (int)(igd.minQuantity + igd.minQuantityIncreasePerLevel * (gpi.CurrentDungeonLevel - 1));
                int max = (int)(igd.maxQuantity + igd.maxQuantityIncreasePerLevel * (gpi.CurrentDungeonLevel - 1));

                for (int i = 0; i < max; i++)
                {
                    double chance = new Random().NextDouble() * 100;

                    if (i < min || chance < igd.spawnChance)
                    {
                        Vector3 pos;
                        do
                        {
                            int x = rnd.Next(1, roomSize - 1);
                            int y = rnd.Next(1, roomSize - 1);
                            pos = roomPos + new Vector3(x, y);
                        } while (usedPositions.Contains(pos));
                        usedPositions.Add(pos);
                        itemSpawner.SpawnItem(igd.itemData, pos);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
