using System;
using System.Collections;
using UnityEngine;
public interface IGridObject
{
    public Action<IGridObject> OnTake { get; set; }
    public void Take();
    public void TakenUpdate(Vector2 targetPosition);
    public void Place(Vector3 pos);
    public void Throw();
}
