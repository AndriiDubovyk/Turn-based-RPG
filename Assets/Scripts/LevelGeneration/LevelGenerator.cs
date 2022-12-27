using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{

    private GridManager gridManager;
    private int[,] templete;
    private int shiftX, shiftY;

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
    private int numberOfEnemyRooms = 3;
    [SerializeField]
    private int enemyRoomMaxCellDistFromEdge = 3;
    [SerializeField]
    private int minCellDistBetweenEnemyRooms = 1;
    [SerializeField]
    private int numberOfInvisibleObstacles = 8; // to make the corridors more undulating
    [SerializeField]
    private int minCellDistBetweenInvisibleObstacles = 1;                                  



    // Start is called before the first frame update
    void Start()
    {
        gridManager = gameObject.GetComponent<GridManager>();
        templete = new TempleteGenerator(levelWidthCells, levelHeightCells, numberOfEnemyRooms, enemyRoomMaxCellDistFromEdge, minCellDistBetweenEnemyRooms, numberOfInvisibleObstacles, minCellDistBetweenInvisibleObstacles).GetTemplete();
        shiftX = - templete.GetLength(0) / 2;
        shiftY = - templete.GetLength(1) / 2;
        ShowWorld(templete);

        CreateGround();
        CreateRooms();
    }

    private void CreateGround()
    {
        for(int i=0; i<templete.GetLength(0); i++)
        {
            for (int j = 0; j < templete.GetLength(1); j++)
            {
                FillCell(gridManager.groundTilemap, defaultGroundTile, new Vector3Int(i , j));
            }
        }
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
                        CreateRoom(x, y);
                        break;
                    case 2:
                        CreateRoom(x, y);
                        break;
                    case 4:
                        CreateCorridor(x, y);
                        break;
                    case 5:
                        CreateRoom(x, y);
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
        if (topSpace != 0)
        {   
            for (int i = 0; i < shift; i++)
            {
                Vector3Int posLeft = new Vector3Int(topLeftCorner.x + shift - 1,
                    topLeftCorner.y+i);
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

    private void CreateRoom(int x, int y)
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
