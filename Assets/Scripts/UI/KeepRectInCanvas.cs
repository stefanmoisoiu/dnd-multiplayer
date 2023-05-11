using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KeepRectInCanvas : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRect;
    
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 worldPosition =
            _canvasRect.InverseTransformPoint(transform.parent.TransformPoint(_rectTransform.anchoredPosition));
        worldPosition = new Vector2
        (
            Mathf.Clamp(worldPosition.x,
                (-_canvasRect.rect.width + _rectTransform.rect.width)/2,
                (_canvasRect.rect.width - _rectTransform.rect.width)/2),
            Mathf.Clamp(worldPosition.y, 
                (-_canvasRect.rect.height + _rectTransform.rect.height)/2,
                (_canvasRect.rect.height - _rectTransform.rect.height)/2)
        );
        _rectTransform.anchoredPosition = transform.parent.InverseTransformPoint(_canvasRect.TransformPoint(worldPosition));
    }
}
