using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour
{
    [SerializeField] private PlayerSignalHandler _signalHandler;
    
    public List<Placable> placedThings;

    private TileMapHolder grid;
    private Preview placablePreview;

    private void Awake()
    {
        placedThings = new List<Placable>();
        _signalHandler.OnMouceClick += ChekClic;
    }

    private void ChekClic()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_signalHandler.MoucePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Props"))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(_signalHandler.MoucePosition);
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
        //if (placablePreview == null)
        //{
        //    return;
        //}

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

        //if (Input.GetMouseButton(0))// åñëè íàæàòà èëè óäåðæèâàåòñÿ ËÊÌ 
        //{
        //    Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2Int gridPos = GetGrid().GetGridPosHere(mouse);

        //    Vector2 cellCenter;
        //    if (GetGrid().IsAreaBounded(gridPos.x, gridPos.y, Vector2Int.one))// â ïðåäåëàõ ëè íàøåé òàáëèöû
        //    {
        //        cellCenter = GetGrid().GetGridCellPosition(gridPos);
        //    }
        //    else
        //    {
        //        cellCenter = mouse;
        //    }

        //    placablePreview.SetCurrentMousePosition(cellCenter, gridPos, () => GetGrid().IsBuildAvailable(gridPos, placablePreview));
        //}
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
        }
    }

    private void OccupyCells(GridPlace place)
    {
        GetGrid().SetGridPlaceStatus(place, true);
    }
}
