using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KeepRectInCanvas : MonoBehaviour
{
    
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Transform parent = _rectTransform.parent;
        Rect canvasRect = MainCanvasManager.MainCanvasRect.rect;
        
        Vector2 worldPosition = MainCanvasManager.LocalToCanvasPos(_rectTransform.anchoredPosition,parent);
        
        worldPosition.x = Mathf.Clamp(worldPosition.x,
            (-canvasRect.width + _rectTransform.rect.width) / 2,
            (canvasRect.width - _rectTransform.rect.width) / 2);
        worldPosition.y = Mathf.Clamp(worldPosition.y,
            (-canvasRect.height + _rectTransform.rect.height) / 2,
            (canvasRect.height - _rectTransform.rect.height) / 2);

        Vector2 localPosition = MainCanvasManager.CanvasToLocalPos(worldPosition, parent);

        _rectTransform.anchoredPosition = localPosition;
    }
}
