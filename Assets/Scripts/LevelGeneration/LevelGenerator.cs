using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

// Unity has different then temlete generator y-axis direction, so there may be some confusion with top or bottom positions
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
    private Tile botWall;
    [SerializeField]
    private Tile leftWall;
    [SerializeField]
    private Tile topWall;
    [SerializeField]
    private Tile rightWall;
    [SerializeField]
    private Tile botLeftCorner;
    [SerializeField]
    private Tile botRightCorner;
    [SerializeField]
    private Tile topLeftCorner;
    [SerializeField]
    private Tile topRightCorner;

    [SerializeField]
    private Tile botRightInnerCorner;
    [SerializeField]
    private Tile botLeftInnerCorner;
    [SerializeField]
    private Tile topRightInnerCorner;
    [SerializeField]
    private Tile topLeftInnerCorner;


    [SerializeField]
    private int templeteCellSize = 17;
    [SerializeField]
    private int corridorWidth = 3;

    //[SerializeField]
    private int levelWidthCells = 9;
    //[SerializeField]
    private int levelHeightCells = 9;
    //[SerializeField]
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
    private Vector3 exitPos;

    [SerializeField]
    private LevelGenerationData levelGenerationData;
    private GameProcessInfo gpi;



    void Awake()
    {
        if (GameObject.Find("GameHandler").GetComponent<GameSaver>().IsSaveExist()) return;

        GameObject[] objs = GameObject.FindGameObjectsWithTag("game_process_info");
        if (objs.Length > 0) gpi = objs[0].GetComponent<GameProcessInfo>();

        numberOfEnemyRooms = (int)(levelGenerationData.startCommonRoomsNumber + levelGenerationData.commonRoomsNumberIncreasePerLevel * (gpi.CurrentDungeonLevel - 1));
        levelWidthCells = levelHeightCells = 6 + (int)(levelGenerationData.startCommonRoomsNumber);

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

    public void GenerateLevelWithTemplete(int[,] templete, Vector3 exitPosition)
    {
        this.templete = templete;
        gridManager = gameObject.GetComponent<GridManager>();

        shiftX = -templete.GetLength(0) / 2;
        shiftY = -templete.GetLength(1) / 2;
        //ShowWorld(templete);
        CreateRooms();
        CreateExit(exitPosition);
    }

    public void GenerateLevelWithTemplete(int[,] templete)
    {
        this.templete = templete;
        gridManager = gameObject.GetComponent<GridManager>();

        shiftX = -templete.GetLength(0) / 2;
        shiftY = -templete.GetLength(1) / 2;
        //ShowWorld(templete);
        CreateRooms();
        CreateExit();
    }


    public Vector3Int GetWorldSize()
    {
        return new Vector3Int(levelWidthCells * templeteCellSize, levelHeightCells * templeteCellSize);
    }

    public int[,] GetLevelTemplete()
    {
        return templete;
    }

    public Vector3 GetExitPosition()
    {
        return exitPos;
    }

    private void CreateExit()
    {
        Random rnd = new Random();
        Vector3 roomPos = GetBossRoomPos();
        int roomSize = GetCellSize();
        int x = rnd.Next(1, roomSize - 1);
        int y = rnd.Next(1, roomSize - 1);
        Vector3 pos = roomPos + new Vector3(x, y);
        levelExit.transform.position = pos;
        exitPos = pos;
        Instantiate(levelExit);
    }

    private void CreateExit(Vector3 pos)
    {
        levelExit.transform.position = pos;
        exitPos = pos;
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
                gridManager.collidersTilemap.SetTile(posLeft, leftWall);
                gridManager.collidersTilemap.SetTile(posRight, rightWall);
            }
        }
        else
        {
            for (int i = 0; i < corridorWidth + 2; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + shift + i - 1,
                    topLeftCorner.y + shift - 1);
                gridManager.collidersTilemap.SetTile(pos, botWall);
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
                gridManager.collidersTilemap.SetTile(posLeft, leftWall);
                gridManager.collidersTilemap.SetTile(posRight, rightWall);
            }
        }
        else
        {
            for (int i = 0; i < corridorWidth + 2; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + shift + i - 1,
                    topLeftCorner.y + templeteCellSize - shift);
                gridManager.collidersTilemap.SetTile(pos, topWall);
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
                gridManager.collidersTilemap.SetTile(posTop, botWall);
                gridManager.collidersTilemap.SetTile(posBot, topWall);
            }
            if(topSpace!=0)
            {
                gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y + shift - 1), botLeftCorner);
            }
            if (bottomSpace != 0)
            {
                gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y + shift + corridorWidth), this.topLeftCorner);
            }
        }
        else
        {
            for (int i = 0; i < corridorWidth + 2; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y + shift + i - 1);
                gridManager.collidersTilemap.SetTile(pos, leftWall);
            }
            if (topSpace == 0)
            {
                gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y + shift - 1), botLeftInnerCorner);
            }
            if (bottomSpace == 0)
            {
                gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y + shift + corridorWidth), topLeftInnerCorner);
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
                gridManager.collidersTilemap.SetTile(posTop, botWall);
                gridManager.collidersTilemap.SetTile(posBot, topWall);
            }
            if (topSpace != 0)
            {
                gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + templeteCellSize - 1 - shift + 1,
                    topLeftCorner.y + shift - 1), botRightCorner);
            }
            if (bottomSpace != 0)
            {
                gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + templeteCellSize - 1 - shift + 1,
                    topLeftCorner.y + shift + corridorWidth), topRightCorner);
            }
        }
        else
        {
            for (int i = 0; i < corridorWidth + 2; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + templeteCellSize - shift,
                    topLeftCorner.y + shift + i - 1);
                gridManager.collidersTilemap.SetTile(pos, rightWall);
            }
            if (topSpace == 0)
            {
                gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + templeteCellSize - shift,
                    topLeftCorner.y + shift - 1), botRightInnerCorner);
            }
            if (bottomSpace == 0)
            {
                gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + templeteCellSize - shift,
                    topLeftCorner.y + shift + corridorWidth), topRightInnerCorner);
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
        FillRoom(gridManager.collidersTilemap, new Vector3Int(x, y), true);
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
            gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y), botLeftCorner);
            gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + shift + corridorWidth,
                    topLeftCorner.y), botRightCorner);
        }
        if (bottomSpace != 0)
        {
            for (int i = 0; i < corridorWidth; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + shift + i,
                    topLeftCorner.y + templeteCellSize - 1);
                gridManager.collidersTilemap.SetTile(pos, null);
            }
            gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y + templeteCellSize - 1), this.topLeftCorner);
            gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + shift + corridorWidth,
                    topLeftCorner.y + templeteCellSize - 1), topRightCorner);
        }
        if (leftSpace != 0)
        {
            for (int i = 0; i < corridorWidth; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x,
                    topLeftCorner.y + shift + i);
                gridManager.collidersTilemap.SetTile(pos, null);
            }
            gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x,
                    topLeftCorner.y + shift -1 ), botLeftCorner);
            gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x,
                    topLeftCorner.y + shift + corridorWidth), this.topLeftCorner);
        }
        if (rightSpace != 0)
        {
            for (int i = 0; i < corridorWidth; i++)
            {
                Vector3Int pos = new Vector3Int(topLeftCorner.x + templeteCellSize - 1,
                    topLeftCorner.y + shift + i);
                gridManager.collidersTilemap.SetTile(pos, null);
            }
            gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + templeteCellSize - 1,
                    topLeftCorner.y + shift + -1), botRightCorner);
            gridManager.collidersTilemap.SetTile(new Vector3Int(topLeftCorner.x + templeteCellSize - 1,
                    topLeftCorner.y + shift + corridorWidth), topRightCorner);
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

    private void FillRoom(Tilemap tilemmap, Vector3Int cellPoint, bool onlyOutline = false)
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
                Tile tile = null;
                if (x == 0 && y == 0) tile = botLeftInnerCorner;
                else if (x == 0 && y == templeteCellSize - 1) tile = topLeftInnerCorner;
                else if (x == templeteCellSize - 1 && y == 0) tile = botRightInnerCorner;
                else if (x == templeteCellSize - 1 && y == templeteCellSize - 1) tile = topRightInnerCorner;
                else if (x == 0) tile = leftWall;
                else if (x == templeteCellSize - 1) tile = rightWall;
                else if (y == templeteCellSize - 1) tile = topWall;
                else if (y == 0) tile = botWall;

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
