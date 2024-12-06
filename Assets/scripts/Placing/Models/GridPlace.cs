public class GridPlace
{
    private Cell[] _place;

    public Cell[] Place { get => _place; set => _place = value; }

    public GridPlace(Cell[] place)
    {
        this._place = place;
    }
}
