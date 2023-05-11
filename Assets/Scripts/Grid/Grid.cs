using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drawing;
using Sirenix.OdinInspector;
using Unity.Mathematics;

public class Grid : MonoBehaviourGizmos
{
    
    [SerializeField] private float gridCellSize,yOffset;
    [SerializeField] private int gridCellCount;

    private List<IGridObject> _gridObjects = new List<IGridObject>();
    private List<Vector2> _gridObjectPositions = new List<Vector2>();
    [FoldoutGroup("Gizmos")] [SerializeField]
    private Color gridGizmoColor = Color.black,sphereGizmoColor;

    

    
    public override void DrawGizmos()
    {
        List<Vector2> gridPositions = GetCenteredGridPositions();
        for (int i = 0; i < gridPositions.Count; i++)
        {
            Draw.ingame.CrossXZ(new float3(transform.position.x + gridPositions[i].x,transform.position.y + yOffset,transform.position.z + gridPositions[i].y),gridCellSize,gridGizmoColor);
            Draw.ingame.CircleXZ(new float3(transform.position.x + gridPositions[i].x,transform.position.y + yOffset,transform.position.z + gridPositions[i].y),gridCellSize/6f,sphereGizmoColor);
        }
    }

    private List<Vector2> GetGridPositions()
    {
        List<Vector2> gridPositions = new();

        for (int i = 0; i < gridCellCount; i++)
        {
            for (int j = 0; j < gridCellCount; j++)
            {
                gridPositions.Add(new Vector2(j,i) * gridCellSize);
            }
        }

        return gridPositions;
    }
    private List<Vector2> GetCenteredGridPositions()
    {
        List<Vector2> gridPositions = new();

        for (int i = 0; i < gridCellCount; i++)
        {
            for (int j = 0; j < gridCellCount; j++)
            {
                gridPositions.Add(new Vector2(j - (float)gridCellCount / 2 + 0.5f,i - (float)gridCellCount / 2 + 0.5f) * gridCellSize);
            }
        }

        return gridPositions;
    }

    public Vector3 GridToWorldPos(Vector2 gridPos)
    {
        Vector3 worldPos = new Vector3(gridPos.x,transform.position.y + yOffset,gridPos.y);
        
        worldPos.x -= (gridCellCount-1) / 2 + 0.5f;
        worldPos.z -= (gridCellCount-1) / 2 + 0.5f;
        worldPos.z *= -1;
        
        worldPos *= gridCellSize;
        
        worldPos.x += transform.position.x;
        worldPos.z += transform.position.z;
        
        return worldPos;
    }
    /// <summary>
    /// Transforms a world position to the grid position. DOES NOT SNAP IT
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector2 WorldToGridPos(Vector3 worldPos)
    {
        worldPos.x -= transform.position.x;
        worldPos.z -= transform.position.z;

        worldPos /= gridCellSize;

        worldPos.x += (gridCellCount-1) / 2 + 0.5f;
        worldPos.z += (gridCellCount-1) / 2 + 0.5f;
        worldPos.z = gridCellCount - 1 - worldPos.z;
        
        return new Vector2(worldPos.x,worldPos.z);
    }

    public Vector2 ClampGridPos(Vector2 gridPos) => new Vector2(Mathf.Clamp(gridPos.x, 0, gridCellCount - 1),
        Mathf.Clamp(gridPos.y, 0, gridCellCount - 1));
    public Vector2 SnapGridPos(Vector2 gridPos)
    {
        gridPos = ClampGridPos(gridPos);
        return new(Mathf.Round(gridPos.x), Mathf.Round(gridPos.y));
    }
    public Vector3 SnapWorldPos(Vector3 worldPos) => GridToWorldPos(SnapGridPos(WorldToGridPos(worldPos)));

    public bool IsGridPositionAvailable(Vector2 pos) => !_gridObjectPositions.Contains(pos);

    public IGridObject GetGridObjectAtGridPos(Vector2 pos) => _gridObjects[_gridObjectPositions.IndexOf(pos)];

    public bool PlaceGridObject(IGridObject gridObject, Vector2 position)
    {
        if (!IsGridPositionAvailable(position)) return false;
        _gridObjects.Add(gridObject);
        _gridObjectPositions.Add(position);

        gridObject.OnTake += OnGridObjectTaken;
        gridObject.Place(GridToWorldPos(position));
        
        return true;
    }

    private void OnGridObjectTaken(IGridObject gridObject)
    {
        for (int i = 0; i < _gridObjects.Count; i++)
        {
            if (_gridObjects[i] != gridObject) continue;
            gridObject.OnTake -= OnGridObjectTaken;
            _gridObjects.RemoveAt(i);
            _gridObjectPositions.RemoveAt(i);
            return;
        }
    }
}
