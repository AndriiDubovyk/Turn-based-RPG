using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private int numberOfEnemiesInCommonRoom = 2;
    [SerializeField]
    private int numberOfEnemiesInBossRoom = 3;

    private LevelGenerator lg;

    void Awake()
    {
        numberOfEnemiesInBossRoom += GameProcessInfo.CurrentLevel / 3;
    }

    // Start is called before the first frame update
    void Start()
    {
        lg = gameObject.GetComponent<LevelGenerator>();
        SpawnEnemiesInCommonRooms();
        SpawnEnemiesInBossRoom();
    }

    void SpawnEnemiesInBossRoom()
    {
        Random rnd = new Random();
        Vector3 roomPos = lg.GetBossRoomPos();
        int roomSize = lg.GetCellSize();
        List<Vector3> usedPositions = new List<Vector3>();
        for (int i = 0; i < numberOfEnemiesInBossRoom; i++)
        {
            Vector3 pos;
            do
            {
                int x = rnd.Next(1, roomSize - 1);
                int y = rnd.Next(1, roomSize - 1);
                pos = roomPos + new Vector3(x, y);
            } while (usedPositions.Contains(pos));
            usedPositions.Add(pos);
            enemy.transform.position = pos;
            Instantiate(enemy);
        }
    }

    void SpawnEnemiesInCommonRooms()
    {
        Random rnd = new Random();
        List<Vector3> roomsPos = lg.GetCommonRoomPos();
        for(int j = 0; j< roomsPos.Count; j++)
        {
            Vector3 roomPos = roomsPos[j];
            int roomSize = lg.GetCellSize();
            List<Vector3> usedPositions = new List<Vector3>();
            for (int i = 0; i < numberOfEnemiesInCommonRoom; i++)
            {
                Vector3 pos;
                do
                {
                    int x = rnd.Next(1, roomSize - 1);
                    int y = rnd.Next(1, roomSize - 1);
                    pos = roomPos + new Vector3(x, y);
                } while (usedPositions.Contains(pos));
                usedPositions.Add(pos);
                enemy.transform.position = pos;
                Instantiate(enemy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
