using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Grid
{
    private GridTile[,] gridArray;

    public Grid(int offsetX, int offsetY, int width, int height, Tilemap collidersTilemap)
    {
        gridArray = new GridTile[width, height];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Vector3Int realTilePos = new Vector3Int(x + offsetX, y + offsetY);
                // allow movement if there no colliders
                gridArray[x, y] = new GridTile(x, y, collidersTilemap.GetTile(realTilePos) == null);
            }
        }
    }

    public List<GridTile> GetNeighbours(GridTile tile)
    {
        List<GridTile> neighbours = new List<GridTile>();
        // Check for left neighbour
        int leftNeighbourIndexX = tile.x - 1;
        int leftNeighbourIndexY = tile.y;
        if (leftNeighbourIndexX > 0 && gridArray[leftNeighbourIndexX, leftNeighbourIndexY].isMovementAllowed)
        {
            neighbours.Add(gridArray[leftNeighbourIndexX, leftNeighbourIndexY]);
        }
        // Check for right neighbour
        int rightNeighbourIndexX = tile.x + 1;
        int rightNeighbourIndexY = tile.y;
        if (leftNeighbourIndexX > 0 && gridArray[rightNeighbourIndexX, rightNeighbourIndexY].isMovementAllowed)
        {
            neighbours.Add(gridArray[rightNeighbourIndexX, rightNeighbourIndexY]);
        }
        // Check for top neighbour
        int topNeighbourIndexX = tile.x;
        int topNeighbourIndexY = tile.y + 1;
        if (leftNeighbourIndexX > 0 && gridArray[topNeighbourIndexX, topNeighbourIndexY].isMovementAllowed)
        {
            neighbours.Add(gridArray[topNeighbourIndexX, topNeighbourIndexY]);
        }
        // Check for bottom neighbour
        int bottomNeighbourIndexX = tile.x;
        int bottomNeighbourIndexY = tile.y - 1;
        if (leftNeighbourIndexX > 0 && gridArray[bottomNeighbourIndexX, bottomNeighbourIndexY].isMovementAllowed)
        {
            neighbours.Add(gridArray[bottomNeighbourIndexX, bottomNeighbourIndexY]);
        }
        return neighbours;
    }

    public GridTile Get(int x, int y)
    {
        return gridArray[x, y];
    }

}