using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Pathfinder
{
    private Grid grid;
    private int offsetX;
    private int offsetY;

    public Pathfinder(int offsetX, int offsetY, int width, int height, Tilemap collidersTilemap, List<Vector3Int> otherUnitsPositions)
    {
        this.grid = new Grid(offsetX, offsetY, width, height, collidersTilemap, otherUnitsPositions);
        this.offsetX = offsetX;
        this.offsetY = offsetY;
    }

    // A* pathfinding
    public List<Vector3Int> GetPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.Get(startX, startY);
        PathNode endNode = grid.Get(endX, endY);
        startNode.gCost = 0;
        startNode.hCost = CalculateHCost(startNode, endNode);
        startNode.CalculateFCost();

        // If we can't reach node we have no path
        if(!endNode.isWalkable || grid.GetNeighbours(endNode).Count == 0)
        {
            return null;
        }
        // No path to the same node
        if (startNode == endNode)
        {
            return null;
        }

        // Nodes that are processing now

        MinPriorityQueue<PathNode> openList = new MinPriorityQueue<PathNode>();
        openList.Enqueue(startNode);
        // Nodes that have been already processed
        HashSet<PathNode> closedList = new HashSet<PathNode>();

        while(openList.Count > 0)
        {
            PathNode currentNode = openList.Dequeue();
            if (currentNode == endNode)
            {
                // We have reached needed node
                return CalculatePathFromEndNode(endNode);
            }

            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in grid.GetNeighbours(currentNode))
            {
                if(closedList.Contains(neighbourNode))
                {
                    continue;
                }
                if(!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + 1;
                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.parent = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateHCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();
                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Enqueue(neighbourNode);
                    }
                }
            }
        }
        return null; // No path was found
    }

    private PathNode GetLowestFCostNode(List<PathNode> list)
    {
        PathNode lowestFCostNode = list[0];
        for(int i=0; i<list.Count; i++)
        {
            if(list[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = list[i];
            }
        }
        return lowestFCostNode;
    }

    private List<Vector3Int> CalculatePathFromEndNode(PathNode endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        PathNode current = endNode;
        while(current.parent != null)
        {
            path.Add(new Vector3Int(current.x + offsetX, current.y + offsetY));
            current = current.parent;
        }
        path.Reverse();
        return path;
    }


    private int CalculateHCost(PathNode current, PathNode target)
    {
        // Use Manhattan Distance because we we are allowed to move only in four directions only
        return Math.Abs(target.x - current.x) + Math.Abs(target.y- current.y);
    }
 
}
