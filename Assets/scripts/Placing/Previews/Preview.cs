using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Preview : MonoBehaviour
{
    [SerializeField] private Placable _placeble;

    public Vector2Int Size;
    public Vector2Int _currentGridPos;

    private bool _isPlacindAvailable;
    private bool _idMoving;

    protected SpriteRenderer _mainRenderer;
    private Color _green;
    private Color _red;

    private void Awake()
    {
        _mainRenderer = GetComponentInChildren<SpriteRenderer>();
        _green = new Color(0, 1, 0.3f, 0.8f);
        _red = new Color(1, 0.2f, 0.2f, 0.8f);
    }

    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        _idMoving = true;
    }

    private void OnMouseUp()
    {
        _idMoving = false;
    }

    public void SetCurrentMousePosition(Vector2 position, Vector2Int GridPose, Func<Boolean> isBuildAvailable)
    {
        if (_idMoving)
        {
            transform.position = position;
            _currentGridPos = GridPose;
            SetBuildAvailable(isBuildAvailable());
        }
    }

    public void SetSpawnPosition(Vector2Int GridPose)
    {
        _currentGridPos = GridPose;
    }

    public Placable InstantiateHere()
    {
        if (_isPlacindAvailable)
        {
            Vector2Int size = GetSize();
            Cell[] placeInGrid = new Cell[size.x * size.y];
            int index = 0;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    placeInGrid[index++] = new Cell(_currentGridPos.x + x, _currentGridPos.y + y);
                }
            }

            Placable placable = InitPlacable(placeInGrid);
            Destroy(gameObject);
            return placable;
        }
        return null;
    }

    private Placable InitPlacable(Cell[] placeInGrid)
    {
        Placable placable = Instantiate(_placeble, transform.position, Quaternion.identity);
        placable.GridPlace = new GridPlace(placeInGrid);
        return placable;
    }

    public void SetBuildAvailable(bool available)
    {
        _isPlacindAvailable = available;
        _mainRenderer.material.color = available ? _green : _red;
    }

    public bool IsBuildAvailable()
        => _isPlacindAvailable;

    public virtual Vector2Int GetSize()
        => Size;
}
