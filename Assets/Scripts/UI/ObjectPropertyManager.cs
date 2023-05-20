using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPropertyManager : MonoBehaviour
{
    [SerializeField] private RectTransform propertyWindow;
    private Camera _uiCam,_cam;

    private void Start()
    {
        _cam = Camera.main;
        _uiCam = GameObject.FindGameObjectWithTag("UI Cam").GetComponent<Camera>();
        InputManager.OnSecondaryStart += OpenPropertyWindow;
    }
    private void OpenPropertyWindow()
    {
        if (!Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) || !hit.transform.TryGetComponent(out MoveObject moveObject))
        {
            propertyWindow.gameObject.SetActive(false);
            return;
        }
        propertyWindow.gameObject.SetActive(true);
        Vector2 offset = new (propertyWindow.rect.width / 2, - propertyWindow.rect.height / 2);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(MainCanvasManager.MainCanvasRect,
            _cam.WorldToScreenPoint(hit.transform.position), _uiCam, out Vector2 objectPos);
        propertyWindow.anchoredPosition = objectPos + offset;
    }
}
