using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemyGenerator : MonoBehaviour
{

    [System.Serializable]
    public class EnemySpawn
    {
        public UnitData unitData;
        public int quantity;
    }

    [SerializeField]
    private List<EnemySpawn> commonRoomEnemies;
    [SerializeField]
    private List<EnemySpawn> bossRoomEnemies;

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
        foreach (EnemySpawn es in bossRoomEnemies)
        {
            for (int i = 0; i < es.quantity; i++)
            {
                Vector3 pos;
                do
                {
                    int x = rnd.Next(1, roomSize - 1);
                    int y = rnd.Next(1, roomSize - 1);
                    pos = roomPos + new Vector3(x, y);
                } while (usedPositions.Contains(pos));
                usedPositions.Add(pos);
                unitSpawner.SpawnUnit(es.unitData, pos);
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
            foreach (EnemySpawn es in commonRoomEnemies)
            {
                for (int i = 0; i < es.quantity; i++)
                {
                    Vector3 pos;
                    do
                    {
                        int x = rnd.Next(1, roomSize - 1);
                        int y = rnd.Next(1, roomSize - 1);
                        pos = roomPos + new Vector3(x, y);
                    } while (usedPositions.Contains(pos));
                    usedPositions.Add(pos);
                    unitSpawner.SpawnUnit(es.unitData, pos);
                }
            }
        }
    }
}
