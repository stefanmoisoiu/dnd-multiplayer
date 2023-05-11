using UnityEngine;

public class GridSelectionManager : MonoBehaviour
{
    public static IGridObject selectedGridObject;
    [SerializeField] private LayerMask gridLayer,groundLayer;
    
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (selectedGridObject != null) SelectedGridObjectTakenUpdate();
        if (Input.GetMouseButtonDown(0)) Clicked();
    }

    private void SelectedGridObjectTakenUpdate()
    {
        if (selectedGridObject == null) return;
        
        Vector2 targetPos = Vector2.zero;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit gridHit, float.MaxValue,gridLayer)
            && gridHit.transform.TryGetComponent(out Grid grid))
        {
            Vector3 snappedWorldPos = grid.SnapWorldPos(gridHit.point);
            targetPos = new Vector2(snappedWorldPos.x, snappedWorldPos.z);
        }
        else if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit groundHit, float.MaxValue,groundLayer))
        {
            targetPos = new Vector2(groundHit.point.x, groundHit.point.z);
        }
        selectedGridObject.TakenUpdate(targetPos);
    }
    private void Clicked()
    {
        if (selectedGridObject == null)
        {
            if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) return;
            if (!hit.transform.TryGetComponent(out IGridObject gridObject)) return;
            TakeGridObject(gridObject);
        }
        else
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit gridHit,float.MaxValue,gridLayer)
                && gridHit.transform.TryGetComponent(out Grid grid)) PlaceGridObject(grid, gridHit.point);
            else
            {
                ThrowGridObject();
            }
        }
    }

    private void TakeGridObject(IGridObject gridObject)
    {
        gridObject.Take();
        selectedGridObject = gridObject;
    }

    private void SwapGridObject(Grid grid,IGridObject gridObject,Vector3 hitPoint)
    {
        gridObject.Take();
        Vector2 snappedGridPos = grid.SnapGridPos(grid.WorldToGridPos(hitPoint));
        grid.PlaceGridObject(selectedGridObject,snappedGridPos);
        selectedGridObject = gridObject;
    }

    private void PlaceGridObject(Grid grid,Vector3 hitPoint)
    {
        if (selectedGridObject == null) return;
        Vector2 snappedGridPos = grid.SnapGridPos(grid.WorldToGridPos(hitPoint));
        if (grid.PlaceGridObject(selectedGridObject, snappedGridPos))
        {
            selectedGridObject = null;
        }
        else
        {
            SwapGridObject(grid,grid.GetGridObjectAtGridPos(snappedGridPos),hitPoint);
        }
    }

    private void ThrowGridObject()
    {
        selectedGridObject.Throw();
        selectedGridObject = null;
    }
}
