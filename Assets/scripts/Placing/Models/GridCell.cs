public class GridCell
{
    public float _centerX;
    public float _centerY;
    public bool IsOccupied;

    public GridCell(float x, float y, bool isOccupied)
    {
        this._centerX = x;
        this._centerY = y;
        this.IsOccupied = isOccupied;
    }
}
