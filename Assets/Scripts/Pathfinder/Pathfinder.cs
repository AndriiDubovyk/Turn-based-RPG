using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Pathfinder
{
    private Grid grid;
    private int endX;
    private int endY;
    private int offsetX;
    private int offsetY;

    public Pathfinder(int offsetX, int offsetY, int width, int height, Tilemap collidersTilemap)
    {
        this.grid = new Grid(offsetX, offsetY, width, height, collidersTilemap);
        this.offsetX = offsetX;
        this.offsetY = offsetY;
    }

    // A* pathfinding
    public List<Vector3Int> GetPath(int startX, int startY, int endX, int endY)
    {
        this.endX = endX;
        this.endY = endY;

        // Initialize the open and closed lists
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        // Put the starting node on the open list
        PathNode startNode = new PathNode(grid.Get(startX, startY), 0, getHCost(startX, startY), null);
        openList.Add(startNode);

        // While the open list is not empty
        while (openList.Count > 0)
        {
            // Find the node with the least f on the open list
            int lowestFCost = openList.Min(a => a.fCost);
            PathNode q = openList.First(a => a.fCost == lowestFCost);
            // Pop q off the open list
            openList.Remove(q);

            // Generate q's successors and set their  parents to q
            List<PathNode> successors = grid.GetNeighbours(q.gridTile)
                .Select(a => new PathNode(
                    a, // gridTile
                    q.gCost + 1, // gCost
                    getHCost(a.x, a.y), // hCost
                    q
                    )).ToList();

            // For each successor
            foreach (PathNode s in successors)
            {
                // If successor is the goal
                if (s.gridTile.x == endX && s.gridTile.y == endY)
                {
                    // Stop search, we have found path
                    return CalculatePathFromEndNode(s);
                }

                // If same position node is in the OPEN list which has a lower f, skip this successor
                bool hasLowerFInOpen = openList.Any(a =>
                {
                    return a.gridTile.x == s.gridTile.x
                    && a.gridTile.y == s.gridTile.y
                    && a.fCost < s.fCost;
                });
                if (hasLowerFInOpen)
                {
                    continue;
                }

                // If same position node is in the CLOSED  list which has a lower f, skip this successor
                bool hasLowerFInClosed = closedList.Any(a =>
                {
                    return a.gridTile.x == s.gridTile.x
                    && a.gridTile.y == s.gridTile.y
                    && a.fCost < s.fCost;
                });
                if (hasLowerFInClosed)
                {
                    continue;
                }

                // If we pass all condition add this successor to the open list
                openList.Add(s);
            }
            // Push q on the closed list
            closedList.Add(q);
        }
        // No path
        return null;

    }

    private List<Vector3Int> CalculatePathFromEndNode(PathNode endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        PathNode current = endNode;
        while(current.parent != null)
        {
            path.Add(new Vector3Int(current.gridTile.x + offsetX, current.gridTile.y + offsetY));
            current = current.parent;
        }
        path.Reverse();
        return path;
    }


    private int getHCost(int currentX, int currentY)
    {
        // Use Manhattan Distance because we we are allowed to move only in four directions only
        return Math.Abs(endX - currentX) + Math.Abs(endY - currentY);
    }
 
}
