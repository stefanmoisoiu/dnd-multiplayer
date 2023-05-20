using UnityEngine;

public class MoveObjectManager : MonoBehaviour
{
    public static MoveObject SelectedObject;
    [SerializeField] private LayerMask gridLayer,groundLayer;
    
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
        InputManager.OnPrimaryStart += Clicked;
    }

    private void Update()
    {
        if (SelectedObject != null) SelectedObjectTakenUpdate();
    }

    private void SelectedObjectTakenUpdate()
    {
        if (SelectedObject == null) return;
        
        Vector2 targetPos = Vector2.zero;
        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit gridHit, float.MaxValue,gridLayer)
            && gridHit.transform.TryGetComponent(out Grid grid))
        {
            Vector3 snappedWorldPos = grid.GetSnappedWorldPos(gridHit.point);
            targetPos = new Vector2(snappedWorldPos.x, snappedWorldPos.z);
        }
        else if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit groundHit, float.MaxValue,groundLayer))
        {
            targetPos = new Vector2(groundHit.point.x, groundHit.point.z);
        }
        SelectedObject.TakenUpdate(targetPos);
    }
    private void Clicked()
    {
        if (SelectedObject == null)
        {
            if (!Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) return;
            if (!hit.transform.TryGetComponent(out MoveObject moveObject)) return;
            TakeObject(moveObject);
        }
        else
        {
            if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit gridHit, float.MaxValue,
                    gridLayer)
                && gridHit.transform.TryGetComponent(out Grid grid))
            {
                Vector2 snappedGridPos = grid.GetSnappedGridPos(gridHit.point);
                GridObject selectedGridObject = new (ref SelectedObject, snappedGridPos);
                
                grid.PlaceOrSwapMoveObject(selectedGridObject,out MoveObject swappedOrNullMoveObject);
                
                SelectedObject = swappedOrNullMoveObject;
            }
            else
            {
                ThrowSelectedObject();
            }
        }
    }

    private void TakeObject(MoveObject moveObject)
    {
        moveObject.Take();
        SelectedObject = moveObject;
    }
    private void ThrowSelectedObject()
    {
        SelectedObject.Throw();
        SelectedObject = null;
    }
}
