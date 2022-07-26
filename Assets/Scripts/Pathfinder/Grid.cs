using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Grid
{
    private PathNode[,] gridArray;

    public Grid(int offsetX, int offsetY, int width, int height, Tilemap collidersTilemap)
    {
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
    }

    public List<PathNode> GetNeighbours(PathNode node)
    {
        int width = gridArray.GetLength(0);
        int height = gridArray.GetLength(1);

        List<PathNode> neighbours = new List<PathNode>();
        // Check for left neighbour
        if (node.x - 1 > 0)
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
        if (node.y - 1 > 0)
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