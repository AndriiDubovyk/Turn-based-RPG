using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class LevelGenerator : MonoBehaviour
{

    private GridManager gridManager;
    private int[,] templete;
    private int shiftX, shiftY;


    [SerializeField]
    private GameObject levelExit;
    [SerializeField]
    private Tile defaultGroundTile;
    [SerializeField]
    private Tile defaultObstaclesTile;

    [SerializeField]
    private int templeteCellSize = 17;
    [SerializeField]
    private int corridorWidth = 3;

    [SerializeField]
    private int levelWidthCells = 9;
    [SerializeField]
    private int levelHeightCells = 9;
    [SerializeField]
    private int numberOfEnemyRooms = 2;
    [SerializeField]
    private int enemyRoomMaxCellDistFromEdge = 3;
    [SerializeField]
    private int minCellDistBetweenEnemyRooms = 1;
    [SerializeField]
    private int numberOfInvisibleObstacles = 8; // to make the corridors more undulating
    [SerializeField]
    private int minCellDistBetweenInvisibleObstacles = 1;

    private Vector3 startRoomPos;
    private Vector3 bossRoomPos;
    private List<Vector3> commonRoomsPos = new List<Vector3>();



    void Awake()
    {
        if (GameObject.Find("GameHandler").GetComponent<GameSaver>().IsSaveExist()) return;
        numberOfEnemyRooms += GameProcessInfo.CurrentLevel / 2;

        templete = new TempleteGenerator(levelWidthCells, levelHeightCells, numberOfEnemyRooms, enemyRoomMaxCellDistFromEdge, minCellDistBetweenEnemyRooms, numberOfInvisibleObstacles, minCellDistBetweenInvisibleObstacles).GetTemplete();
        // generate level again with bigger size
        while (templete == null)
        {
            levelWidthCells += 1;
            levelHeightCells += 1;
            templete = new TempleteGenerator(levelWidthCells, levelHeightCells, numberOfEnemyRooms, enemyRoomMaxCellDistFromEdge, minCellDistBetweenEnemyRooms, numberOfInvisibleObstacles, minCellDistBetweenInvisibleObstacles).GetTemplete();
        }

        GenerateLevelWithTemplete(templete);
    }

    public void GenerateLevelWithTemplete(int[,] templete)
    {
        this.templete = templete;
        gridManager = gameObject.GetComponent<GridManager>();

        shiftX = -templete.GetLength(0) / 2;
        shiftY = -templete.GetLength(1) / 2;
        ShowWorld(templete);
        CreateRooms();
        CreateExit();
    }

    public int[,] GetLevelTemplete()
    {
        return templete;
    }

    private void CreateExit()
    {
        Random rnd = new Random();
        Vector3 roomPos = GetBossRoomPos();
        int roomSize = GetCellSize();
        List<Vector3> usedPositions = new List<Vector3>();
        Vector3 pos;
        do
        {
            int x = rnd.Next(1, roomSize - 1);
            int y = rnd.Next(1, roomSize - 1);
            pos = roomPos + new Vector3(x, y);
        } while (usedPositions.Contains(pos));
        usedPositions.Add(pos);
        levelExit.transform.position = pos;
        Instantiate(levelExit);
    }

    private void CreateRooms()
    {
        for (int x = 0; x < templete.GetLength(0); x++)
        {
            for (int y = 0; y < templete.GetLength(1); y++)
            {
                switch(templete[x,y])
                {
                    case 1:
                        startRoomPos = CreateRoom(x, y) + new Vector3(0.5f, 0.5f);
                        break;
                    case 2:
                        commonRoomsPos.Add(CreateRoom(x, y) + new Vector3(0.5f, 0.5f));
                        break;
                    case 4:
                        CreateCorridor(x, y);
                        break;
                    case 5:
                        bossRoomPos = CreateRoom(x, y) + new Vector3(0.5f, 0.5f);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void CreateCorridor(int x, int y)
    {
        int width = templete.GetLength(0);
        int height = templete.GetLength(1);

        int topSpace = y > 0 ? templete[x, y-1] : 0;
        int bottomSpace = y < height - 1 ? templete[x, y+1] : 0;
        int leftSpace = x > 0 ? templete[x-1, y] : 0;
        int rightSpace = x < width - 1 ? templete[x+1, y] : 0;

        int shift = (templeteCellSize - corridorWidth) / 2;

        Vector3Int topLeftCorner = new Vector3Int((x + shiftX) * templeteCellSize, (y + shiftY) * templeteCellSize);

        // floor at the center
        for(int i=0; i<corridorWidth; i++)
        {
            for (int j = 0; j < corridorWidth; j++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + shift + i,
                    topLeftCorner.y + +shift + j);
                gridManager.groundTilemap.SetTile(pos, defaultGroundTile);
            }
        }

        if (topSpace != 0)
        {   
            for (int i = 0; i < shift; i++)
            {
                Vector3Int posLeft = new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y+i);
                for(int j = 0; j<corridorWidth; j++)
                {
                    Vector3Int pos = new Vector3Int(topLeftCorner.x + shift +j,
                    topLeftCorner.y + i);
                    gridManager.groundTilemap.SetTile(pos, defaultGroundTile);
                }
                Vector3Int posRight = new Vector3Int(topLeftCorner.x + shift + corridorWidth,
                    topLeftCorner.y+i);
                gridManager.collidersTilemap.SetTile(posLeft, defaultObstaclesTile);
                gridManager.collidersTilemap.SetTile(posRight, defaultObstaclesTile);
            }
        }
        else
        {
            for (int i = 0; i < corridorWidth + 2; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + shift + i - 1,
                    topLeftCorner.y + shift - 1);
                gridManager.collidersTilemap.SetTile(pos, defaultObstaclesTile);
            }
        }
        if (bottomSpace != 0)
        {
            for (int i = 0; i < shift; i++)
            {
                Vector3Int posLeft = new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y+templeteCellSize-1-i);
                for (int j = 0; j < corridorWidth; j++)
                {
                    Vector3Int pos = new Vector3Int(topLeftCorner.x + shift + j,
                    topLeftCorner.y + templeteCellSize - 1 - i);
                    gridManager.groundTilemap.SetTile(pos, defaultGroundTile);
                }
                Vector3Int posRight = new Vector3Int(topLeftCorner.x + shift + corridorWidth,
                    topLeftCorner.y + templeteCellSize - 1 - i);
                gridManager.collidersTilemap.SetTile(posLeft, defaultObstaclesTile);
                gridManager.collidersTilemap.SetTile(posRight, defaultObstaclesTile);
            }
        }
        else
        {
            for (int i = 0; i < corridorWidth + 2; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + shift + i - 1,
                    topLeftCorner.y + templeteCellSize - shift);
                gridManager.collidersTilemap.SetTile(pos, defaultObstaclesTile);
            }
        }
        if (leftSpace != 0)
        {
            for (int i = 0; i < shift; i++)
            {
                Vector3Int posTop = new Vector3Int(topLeftCorner.x+i,
                    topLeftCorner.y + shift - 1);
                for (int j = 0; j < corridorWidth; j++)
                {
                    Vector3Int pos = new Vector3Int(topLeftCorner.x + i,
                    topLeftCorner.y + shift + j);
                    gridManager.groundTilemap.SetTile(pos, defaultGroundTile);
                }
                Vector3Int posBot = new Vector3Int(topLeftCorner.x+i,
                    topLeftCorner.y + shift + corridorWidth);
                gridManager.collidersTilemap.SetTile(posTop, defaultObstaclesTile);
                gridManager.collidersTilemap.SetTile(posBot, defaultObstaclesTile);
            }
        }
        else
        {
            for (int i = 0; i < corridorWidth + 2; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y + shift + i - 1);
                gridManager.collidersTilemap.SetTile(pos, defaultObstaclesTile);
            }
        }
        if (rightSpace != 0)
        {
            for (int i = 0; i < shift; i++)
            {
                Vector3Int posTop = new Vector3Int(topLeftCorner.x + templeteCellSize - 1 - i,
                    topLeftCorner.y + shift - 1);
                for (int j = 0; j < corridorWidth; j++)
                {
                    Vector3Int pos = new Vector3Int(topLeftCorner.x + templeteCellSize - 1 - i,
                    topLeftCorner.y + shift + j);
                    gridManager.groundTilemap.SetTile(pos, defaultGroundTile);
                }
                Vector3Int posBot = new Vector3Int(topLeftCorner.x + templeteCellSize - 1 - i,
                    topLeftCorner.y + shift + corridorWidth);
                gridManager.collidersTilemap.SetTile(posTop, defaultObstaclesTile);
                gridManager.collidersTilemap.SetTile(posBot, defaultObstaclesTile);
            }
        }
        else
        {
            for (int i = 0; i < corridorWidth + 2; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + templeteCellSize - shift,
                    topLeftCorner.y + shift + i - 1);
                gridManager.collidersTilemap.SetTile(pos, defaultObstaclesTile);
            }
        }
    }

    public int GetCellSize()
    {
        return templeteCellSize;
    }

    public Vector3 GetStartRoomPos()
    {
        return startRoomPos;
    }

    public Vector3 GetBossRoomPos()
    {
        return bossRoomPos;
    }

    public List<Vector3> GetCommonRoomPos()
    {
        return commonRoomsPos;
    }

    private Vector3Int CreateRoom(int x, int y) // return room coords
    {
        FillCell(gridManager.collidersTilemap, defaultObstaclesTile, new Vector3Int(x, y), true);
        int width = templete.GetLength(0);
        int height = templete.GetLength(1);

        int topSpace = y > 0 ? templete[x, y - 1] : 0;
        int bottomSpace = y < height - 1 ? templete[x, y + 1] : 0;
        int leftSpace = x > 0 ? templete[x - 1, y] : 0;
        int rightSpace = x < width - 1 ? templete[x + 1, y] : 0;

        int shift = (templeteCellSize - corridorWidth) / 2;

        Vector3Int topLeftCorner = new Vector3Int((x + shiftX) * templeteCellSize, (y + shiftY) * templeteCellSize);
        if (topSpace != 0)
        {
            for (int i = 0; i < corridorWidth; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + shift + i,
                    topLeftCorner.y);
                gridManager.collidersTilemap.SetTile(pos, null);
            }
        }
        if (bottomSpace != 0)
        {
            for (int i = 0; i < corridorWidth; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + shift + i,
                    topLeftCorner.y + templeteCellSize - 1);
                gridManager.collidersTilemap.SetTile(pos, null);
            }
        }
        if (leftSpace != 0)
        {
            for (int i = 0; i < corridorWidth; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x,
                    topLeftCorner.y + shift + i);
                gridManager.collidersTilemap.SetTile(pos, null);
            }
        }
        if (rightSpace != 0)
        {
            for (int i = 0; i < corridorWidth; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + templeteCellSize - 1,
                    topLeftCorner.y + shift + i);
                gridManager.collidersTilemap.SetTile(pos, null);
            }
        }
        for (int i = 0; i< templeteCellSize; i++)
        {
            for (int j = 0; j < templeteCellSize; j++)
            {
                gridManager.groundTilemap.SetTile(topLeftCorner + new Vector3Int(i, j), defaultGroundTile);
            }      
        }
        return topLeftCorner;
    }

    private void FillCell(Tilemap tilemmap, Tile tile, Vector3Int cellPoint, bool onlyOutline = false)
    {
        Vector3Int shiftPoint = new Vector3Int(cellPoint.x+shiftX, cellPoint.y+shiftY);
        for(int x=0; x< templeteCellSize; x++)
        {
            for (int y = 0; y < templeteCellSize; y++)
            {
                if (onlyOutline && x != 0 && x != templeteCellSize - 1 && y != 0 && y != templeteCellSize - 1) 
                    continue;
                Vector3Int pos = new Vector3Int(shiftPoint.x * templeteCellSize + x,
                    shiftPoint.y * templeteCellSize + y);
                tilemmap.SetTile(pos, tile);
            }
        }
    }


    private void ShowWorld(int[,] world)
    {
        string worldStr = "";
        for (int y = world.GetLength(1)-1; y >=0; y--) // Reverse 
        {
            for (int x = 0; x < world.GetLength(0); x++)
            {
                worldStr += world[x, y];
            }
            worldStr += "\n";
        }
        Debug.Log(worldStr);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
