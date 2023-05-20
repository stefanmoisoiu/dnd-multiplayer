using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Drawing;
using Sirenix.OdinInspector;
using Unity.Mathematics;

public class Grid : MonoBehaviourGizmos
{
    [SerializeField] private float gridCellSize;
    [SerializeField] private float yGridOffset;
    [SerializeField] private int gridCellCount;

    private List<GridObject> _gridObjects = new();

    [FoldoutGroup("Gizmos")] [SerializeField]
    private Color gridGizmoColor = Color.black;

    [FoldoutGroup("Gizmos")] [SerializeField]
    private bool showGizmos = true;

    

    public Vector3 GetSnappedWorldPos(Vector3 worldPos)
    {
        return GridToWorldPos(GetSnappedGridPos(worldPos));
    }
    public Vector2 GetSnappedGridPos(Vector3 worldPos)
    {
        Vector2 gridPos = WorldToGridPos(worldPos);
        gridPos = ClampGridPos(gridPos);
        gridPos = SnapGridPos(gridPos);
        return gridPos;
    }
    
    public void PlaceMoveObject(GridObject gridObject,out bool succeeded)
    {
        if (GridPosTaken(gridObject.Position))
        {
            succeeded = false;
        }
        else
        {
            _gridObjects.Add(gridObject);
            gridObject.MoveObject.Place(GridToWorldPos(gridObject.Position));
            gridObject.MoveObject.OnTake += OnMoveObjectTake;
            succeeded = true;
        }
    }

    private void OnMoveObjectTake(MoveObject moveObject)
    {
        GridObject gridObjectToRemove = _gridObjects.First(x => x.MoveObject == moveObject);
        _gridObjects.Remove(gridObjectToRemove);
        moveObject.OnTake -= OnMoveObjectTake;
    }
    public void PlaceOrSwapMoveObject(GridObject gridObject, out MoveObject swappedMoveObject)
    {
        PlaceMoveObject(gridObject, out bool succeeded);
        if (succeeded)
        {
            swappedMoveObject = null;
        }
        else
        {
            GridObject swappedGridObject = GetGridObjectAtPos(gridObject.Position);
            swappedGridObject.MoveObject.Take();
            swappedMoveObject = swappedGridObject.MoveObject;
            PlaceMoveObject(gridObject,out bool succeeded2);
        }
    }
    
    // TakeGridObject Was not necessary, if needed can be added.
    
    // public void TakeGridObject(Vector2 gridPos,out bool succeeded)
    // {
    //     if (GridPosTaken(gridPos))
    //     {
    //         succeeded = true;
    //         GridObject gridObject = GetGridObjectAtPos(gridPos);
    //         gridObject.MoveObject.Take();
    //         _gridObjects.Remove(gridObject);
    //     }
    //     else succeeded = false;
    // }
    
    private bool GridPosTaken(Vector2 gridPos) => _gridObjects.Exists(x => x.Position == gridPos);
    private GridObject GetGridObjectAtPos(Vector2 gridPos) =>
        _gridObjects.First(x => x.Position == gridPos);
    private Vector2 ClampGridPos(Vector2 gridPos)
    {
        return new Vector2( Mathf.Clamp(gridPos.x, 0, gridCellCount - 1),
                            Mathf.Clamp(gridPos.y, 0, gridCellCount - 1));
    }
    private Vector2 SnapGridPos(Vector2 gridPos)
    {
        gridPos = ClampGridPos(gridPos);
        return new(Mathf.Round(gridPos.x), Mathf.Round(gridPos.y));
    }
    private Vector2 WorldToGridPos(Vector3 worldPos)
    {
        Vector2 gridPos = new(worldPos.x,worldPos.z);
        gridPos /= gridCellSize;
        gridPos.x += (gridCellCount-1) / 2 + 0.5f;
        gridPos.y *= -1;
        gridPos.y += (gridCellCount-1) / 2 + 0.5f;
        return gridPos;
    }
    private Vector3 GridToWorldPos(Vector2 gridPos)
    {
        Vector3 worldPos = new (gridPos.x,0,gridPos.y);
        
        worldPos.x -= (gridCellCount-1) / 2 + 0.5f;
        worldPos.z -= (gridCellCount - 1) / 2 + 0.5f;
        worldPos.z *= -1;
        
        worldPos.x += transform.position.x;
        worldPos.z += transform.position.z;
        
        worldPos *= gridCellSize;

        worldPos.y = transform.position.y + yGridOffset;
        
        return worldPos;
    }


    public override void DrawGizmos()
    {
        if (!showGizmos) return;
        for (int i = 0; i < gridCellCount + 1; i++)
        {
            for (int j = 0; j < gridCellCount + 1; j++)
            {
                Vector2 pos = new (i - 0.5f, j - 0.5f);
                Draw.ingame.CrossXZ(GridToWorldPos(pos),gridCellSize,gridGizmoColor);
            }
        }
    }
}
public struct GridObject
{
    public MoveObject MoveObject { get; private set; }
    public Vector2 Position { get; private set; }

    public GridObject(ref MoveObject moveObject, Vector2 position)
    {
        MoveObject = moveObject;
        Position = position;
    }
}