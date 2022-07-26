using System;

public class PathNode : IComparable
{
    public int x;
    public int y;
    public bool isWalkable;

    public int gCost;
    public int hCost;
    public int fCost;
    public PathNode parent;

    public PathNode(int x, int y, bool isWalkable)
    {
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
        this.gCost = int.MaxValue;
        this.hCost = int.MaxValue;
        this.fCost = int.MaxValue;
        this.parent = null;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        PathNode otherPathNode = obj as PathNode;
        if (otherPathNode != null)
            return this.fCost.CompareTo(otherPathNode.fCost);
        else
            throw new ArgumentException("Object is not a Temperature");
    }
}