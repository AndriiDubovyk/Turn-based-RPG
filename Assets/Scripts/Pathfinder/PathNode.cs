public class PathNode
{
    public GridTile gridTile;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode parent;

    public PathNode(GridTile gridTile, int gCost, int hCost, PathNode parent)
    {
        this.gridTile = gridTile;
        this.gCost = gCost;
        this.hCost = hCost;
        this.fCost = gCost + hCost;
        this.parent = parent;
    }

}