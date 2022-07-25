public class GridTile
{
    public int x;
    public int y;
    public bool isMovementAllowed;

    public GridTile(int x, int y, bool isMovementAllowed)
    {
        this.x = x;
        this.y = y;
        this.isMovementAllowed = isMovementAllowed;
    }

}