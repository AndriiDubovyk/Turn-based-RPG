using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class UnitsPathfinder : Pathfinder
{

    public UnitsPathfinder(int offsetX, int offsetY, int width, int height, Tilemap collidersTilemap, List<Vector3Int> otherUnitsPositions) : base(offsetX, offsetY, width, height, GetObstacles(offsetX, offsetY, width, height, collidersTilemap, otherUnitsPositions))
    {
        
    }

    private static bool[,] GetObstacles(int offsetX, int offsetY, int width, int height, Tilemap collidersTilemap, List<Vector3Int> otherUnitsPositions)
    {
        bool[,] obstacles = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int realTilePos = new Vector3Int(x + offsetX, y + offsetY);
                // allow movement if there no colliders
                bool isWalkable = collidersTilemap.GetTile(realTilePos) == null;
                obstacles[x, y] = !isWalkable;
            }
        }
        for (int i = 0; i < otherUnitsPositions.Count; i++)
        {
            Vector3Int unitCellAtGrid = new Vector3Int(otherUnitsPositions[i].x - offsetX, otherUnitsPositions[i].y - offsetY);
            if (unitCellAtGrid.x < 0 || unitCellAtGrid.x >= width
                 || unitCellAtGrid.y < 0 || unitCellAtGrid.y >= height)
            {
                continue; // we can't reach this cell anyway
            }
            obstacles[unitCellAtGrid.x, unitCellAtGrid.y] = true;
        }
        return obstacles;
    }

}
