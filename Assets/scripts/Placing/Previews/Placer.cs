using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Placer : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    public List<Placable> placedThings;

    private TileMapHolder grid;
    private Preview placablePreview;

    private bool _isPlaced;
    private Vector3 _moucePosition;

    private void Awake()
    {
        placedThings = new List<Placable>();

        _playerInput.onActionTriggered += OnPlayerInputActionTriggered;
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        InputAction action = context.action;

        switch (action.name)
        {
            case "Target":
                switch (action.phase)
                {
                    case InputActionPhase.Started:
                        ChekClic();
                        break;
                    case InputActionPhase.Canceled:
                        UnClic();
                        break;
                }
                break;

            case "Look":
                _moucePosition = action.ReadValue<Vector2>();
                break;

            case "Enter":
                InstantiatePlacable();
                break;
        }
    }

    private void UnClic()
    {
        _isPlaced = false;
    }

    private void ChekClic()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_moucePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Props"))
        {
            _isPlaced = true;
        }
    }

    private TileMapHolder GetGrid()
    {
        if (grid == null)
        {
            grid = GetComponent<TileMapHolder>();
        }

        return grid;
    }

    private void Update()
    {
        //Debug.Log(_isPlaced);

        if (placablePreview == null)
        {
            return;
        }

        //if (Input.GetMouseButtonDown(1)) // åñëè íàæàòà ÏêÌ, òî îòìåíÿåì ïîñòðîéêó
        //{
        //    Destroy(placablePreview.gameObject);
        //    placablePreview = null;
        //    return;
        //}
        //else if (Input.GetKeyDown(KeyCode.Return))// (KeyCode.KeypadEnter))
        //{
        //    InstantiatePlacable();
        //}

        if (_isPlaced)
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(_moucePosition);
            Vector2Int gridPos = GetGrid().GetGridPosHere(mouse);

            Vector2 cellCenter;
            if (GetGrid().IsAreaBounded(gridPos.x, gridPos.y, Vector2Int.one))// â ïðåäåëàõ ëè íàøåé òàáëèöû
            {
                cellCenter = GetGrid().GetGridCellPosition(gridPos);
            }
            else
            {
                cellCenter = mouse;
            }

            placablePreview.SetCurrentMousePosition(cellCenter, gridPos, () => GetGrid().IsBuildAvailable(gridPos, placablePreview));
        }
    }

    public void ShowPlacablePreview(Preview preview)
    {
        if (placablePreview != null)
        {
            Destroy(placablePreview.gameObject);
        }

        var cameraPos = Camera.main.transform.position;
        var instPreviewPos = new Vector2(cameraPos.x, cameraPos.y);

        placablePreview = Instantiate(preview, instPreviewPos, Quaternion.identity);

        Vector2Int gridPos = GetGrid().GetGridPosHere(placablePreview.transform.position);

        if (GetGrid().IsAreaBounded(gridPos.x, gridPos.y, Vector2Int.one))
        {
            placablePreview.SetSpawnPosition(gridPos);
            placablePreview.SetBuildAvailable(GetGrid().IsBuildAvailable(gridPos, placablePreview));
        }
        else
        {
            placablePreview.SetBuildAvailable(false);
        }
    }

    private void InstantiatePlacable()
    {
        if (placablePreview != null && placablePreview.IsBuildAvailable())
        {
             Placable placableInstance = placablePreview.InstantiateHere();

            placedThings.Add(placableInstance);
            OccupyCells(placableInstance.GridPlace);

            Destroy(placablePreview.gameObject);

            if (placablePreview != null)
            {
                placablePreview = null;
            }
            _isPlaced = false;
        }
    }

    private void OccupyCells(GridPlace place)
    {
        GetGrid().SetGridPlaceStatus(place, true);
    }
}
