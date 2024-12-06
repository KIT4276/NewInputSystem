using UnityEngine;
using UnityEngine.Tilemaps;


    public class TileMapHolder : MonoBehaviour
    {
        public Vector2Int Size;

        private Tilemap _map;
        private GridCell[,] _grid;

        private void Awake()
        {
            _map = GetComponentInChildren<Tilemap>();

            _grid = new GridCell[Size.x, Size.y];
            _map.size = new Vector3Int(Size.x, Size.y, 0);

            Vector3 tilePosition;
            Vector3Int coordinate = new Vector3Int(0, 0, 0);

            for (int x = 0; x < _map.size.x; x++)
            {
                for (int y = 0; y < _map.size.y; y++)
                {
                    coordinate.x = x;
                    coordinate.y = y;
                    tilePosition = _map.CellToWorld(coordinate);
                    _grid[x, y] = new GridCell(tilePosition.x, tilePosition.y, false);
                }
            }
        }

        public void SetGridPlaceStatus(GridPlace place, bool isOccupied)
        {
            foreach (var cell in place.Place)
            {
                _grid[cell._x, cell._y].IsOccupied = isOccupied;
            }
        }


        public Vector2Int GetGridPosHere(Vector3 position)
        {
            Vector3Int cellIndex = _map.WorldToCell(position);
            return new Vector2Int(cellIndex.x, cellIndex.y);
        }

        public Vector2 GetGridCellPosition(Vector2Int indecies)
        {
            if (IsAreaBounded(indecies.x, indecies.y, Vector2Int.one))
            {
                GridCell gridCell = _grid[indecies.x, indecies.y];
                return new Vector2(gridCell._centerX, gridCell._centerY);
            }
            return new Vector2(indecies.x, indecies.y);
        }

        public bool IsAreaBounded(int x, int y, Vector2Int size)
        {
            bool avalible = x >= 0 && x <= _grid.GetLength(0) - size.x;
            if (avalible)
                return y >= 0 && y <= _grid.GetLength(1) - size.y;
            return avalible;
        }

        public bool IsBuildAvailable(Vector2Int gridPose, Preview preview)
        {
            bool available = IsAreaBounded(gridPose.x, gridPose.y, preview.GetSize());
            if (available && IsPlaceTaken(gridPose.x, gridPose.y, preview.GetSize())) { available = false; }

            return available;
        }

        private bool IsPlaceTaken(int placeX, int placeY, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (_grid[placeX + x, placeY + y].IsOccupied) return true;
                }
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            if (_grid != null)
            {
                for (int x = 0; x < _grid.GetLength(0); x++)
                {
                    for (int y = 0; y < _grid.GetLength(1); y++)
                    {
                        if (_grid[x, y] != null)
                        {
                            Gizmos.color = _grid[x, y].IsOccupied ? new Color(1, 0.5f, 0.5f) : new Color(0, 1f, 0.5f);
                            Gizmos.DrawSphere(new Vector3(_grid[x, y]._centerX, _grid[x, y]._centerY, 0), 0.3f);
                        }
                    }
                }
            }
        }
    }

