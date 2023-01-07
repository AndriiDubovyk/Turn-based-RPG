using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ItemGenerator : MonoBehaviour
{

    [System.Serializable]
    public class ItemSpawn
    {
        public ItemData itemData;
        public int quantity;
    }


    [SerializeField]
    private List<ItemSpawn> startRoomItems;
    [SerializeField]
    private List<ItemSpawn> commonRoomItems; // items for ALL rooms - NOT FOR EACH
    [SerializeField]
    private List<ItemSpawn> bossRoomItems;

    [SerializeField]
    private LevelGenerator levelGenerator;
    [SerializeField]
    private ItemSpawner itemSpawner;

    private void Awake()
    {
        if (GameObject.Find("GameHandler").GetComponent<GameSaver>().IsSaveExist()) return;

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
        foreach (ItemSpawn itemSpawn in startRoomItems)
        {
            for(int k=0; k<itemSpawn.quantity; k++)
            {
                Vector3 pos;
                do
                {
                    int x = rnd.Next(1, roomSize - 1);
                    int y = rnd.Next(1, roomSize - 1);
                    pos = roomPos + new Vector3(x, y);
                } while (usedPositions.Contains(pos));
                usedPositions.Add(pos);
                itemSpawner.SpawnItem(itemSpawn.itemData, pos);
            }
        }
    }

    private void SpawnItemsInCommonRooms()
    {
        Random rnd = new Random();
        List<Vector3> roomsPos = levelGenerator.GetCommonRoomPos();
        int roomSize = levelGenerator.GetCellSize();
        List<Vector3> usedPositions = new List<Vector3>();
        foreach (ItemSpawn itemSpawn in commonRoomItems)
        {
            for (int k = 0; k < itemSpawn.quantity; k++)
            {
                Vector3 pos;
                do
                {
                    int roomIndex = rnd.Next(roomsPos.Count);
                    int x = rnd.Next(1, roomSize - 1);
                    int y = rnd.Next(1, roomSize - 1);
                    pos = roomsPos[roomIndex] + new Vector3(x, y, roomIndex);
                } while (usedPositions.Contains(pos));
                usedPositions.Add(pos);
                itemSpawner.SpawnItem(itemSpawn.itemData, pos);
            }       
        }
    }

    private void SpawnItemsInBossRoom()
    {
        Random rnd = new Random();
        Vector3 roomPos = levelGenerator.GetBossRoomPos();
        int roomSize = levelGenerator.GetCellSize();
        List<Vector3> usedPositions = new List<Vector3>();
        foreach (ItemSpawn itemSpawn in bossRoomItems)
        {
            for (int k = 0; k < itemSpawn.quantity; k++)
            {
                Vector3 pos;
                do
                {
                    int x = rnd.Next(1, roomSize - 1);
                    int y = rnd.Next(1, roomSize - 1);
                    pos = roomPos + new Vector3(x, y);
                } while (usedPositions.Contains(pos));
                usedPositions.Add(pos);
                itemSpawner.SpawnItem(itemSpawn.itemData, pos);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
