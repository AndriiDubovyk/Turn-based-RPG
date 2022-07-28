using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Grid
{
    private PathNode[,] gridArray;
    private int offsetX;
    private int offsetY;
    private int width;
    private int height;

    public Grid(int offsetX, int offsetY, int width, int height, Tilemap collidersTilemap, List<Vector3Int> otherUnitsPositions)
    {
        this.offsetX = offsetX;
        this.offsetY = offsetY;
        this.width = width;
        this.height = height;
        gridArray = new PathNode[width, height];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Vector3Int realTilePos = new Vector3Int(x + offsetX, y + offsetY);
                // allow movement if there no colliders
                bool isWalkable = collidersTilemap.GetTile(realTilePos) == null;
                gridArray[x, y] = new PathNode(x, y, isWalkable);
            }
        }
        ProcessUnitsCells(otherUnitsPositions);
    }

    private void ProcessUnitsCells(List<Vector3Int> unitsCells)
    {
        for(int i=0; i<unitsCells.Count; i++)
        {
            Vector3Int unitCellAtGrid = new Vector3Int(unitsCells[i].x - offsetX, unitsCells[i].y - offsetY);
            if(unitCellAtGrid.x<0 || unitCellAtGrid.x >= width
                 || unitCellAtGrid.y<0 || unitCellAtGrid.y >= height)
            {
                continue; // we can't reach this cell anyway
            }
            gridArray[unitCellAtGrid.x, unitCellAtGrid.y].isWalkable = false;
        }
    }
    public List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();
        // Check for left neighbour
        if (node.x - 1 >= 0)
        {
            neighbours.Add(gridArray[node.x - 1, node.y]);
        }
        // Check for right neighbour
        if (node.x + 1 < width)
        {
            neighbours.Add(gridArray[node.x + 1, node.y]);
        }
        // Check for top neighbour
        if (node.y + 1 < height)
        {
            neighbours.Add(gridArray[node.x, node.y + 1]);
        }
        // Check for bottom neighbour
        if (node.y - 1 >= 0)
        {
            neighbours.Add(gridArray[node.x, node.y - 1]);
        }
        return neighbours;
    }

    public PathNode Get(int x, int y)
    {
        return gridArray[x, y];
    }
}