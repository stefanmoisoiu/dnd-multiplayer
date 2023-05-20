using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasManager : MonoBehaviour
{
    public static Canvas MainCanvas;
    public static RectTransform MainCanvasRect;

    private void Start()
    {
        MainCanvas = GetComponent<Canvas>();
        MainCanvasRect = GetComponent<RectTransform>();
    }
    public static Vector2 LocalToCanvasPos(Vector2 anchoredPosition,Transform parent)
    {
        return MainCanvasRect.InverseTransformPoint(parent.TransformPoint(anchoredPosition));
    }

    public static Vector2 CanvasToLocalPos(Vector2 canvasPos, Transform parent)
    {
        return parent.InverseTransformPoint(MainCanvasRect.TransformPoint(canvasPos));
    }
}
