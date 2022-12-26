using System.Collections.Generic;

public class Grid
{
    private PathNode[,] gridArray;
    private int width;
    private int height;

    public Grid(int width, int height, bool[,] obstacles)
    {
        this.width = width;
        this.height = height;
        gridArray = new PathNode[width, height];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new PathNode(x, y, !obstacles[x,y]);
            }
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