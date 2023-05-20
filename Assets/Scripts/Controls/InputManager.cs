using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Controls _controls;
    public static Action OnPrimaryStart, WhilePrimaryHeld, OnPrimaryRelease;
    public static Action OnSecondaryStart, WhileSecondaryHeld, OnSecondaryRelease;

    private void Awake()
    {
        _controls = new Controls();
        _controls.Main.Primary.started += ctx => OnPrimaryStart?.Invoke();
        _controls.Main.Primary.performed += ctx => WhilePrimaryHeld?.Invoke();
        _controls.Main.Primary.canceled += ctx => OnPrimaryRelease?.Invoke();
        _controls.Main.Secondary.started += ctx => OnSecondaryStart?.Invoke();
        _controls.Main.Secondary.performed += ctx => WhileSecondaryHeld?.Invoke();
        _controls.Main.Secondary.canceled += ctx => OnSecondaryRelease?.Invoke();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
