using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameInput : NetworkBehaviour
{
    public event EventHandler OnJump;
    public event EventHandler OnRun;
    public event EventHandler OnRunExit;
    public event EventHandler OnAim;
    public event EventHandler OnAimExit;
    public event EventHandler OnEquip;
    public event EventHandler OnUnequip;

    private PlayerInputActions playerInputActions;

    private void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Player.Jump.started += Jump_performed;
        playerInputActions.Player.Run.performed += Run_performed;
        playerInputActions.Player.Run.canceled += Run_canceled;
        playerInputActions.Gun.Equip.performed += Equip_performed;
        playerInputActions.Gun.Unequip.performed += Unequip_performed;
    }

    private void Unequip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnUnequip?.Invoke(this, EventArgs.Empty);
    }

    private void Equip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnEquip?.Invoke(this, EventArgs.Empty);
    }

    private void Aim_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAimExit?.Invoke(this, EventArgs.Empty);
    }

    private void Aim_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAim?.Invoke(this, EventArgs.Empty);
    }

    private void Run_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnRunExit?.Invoke(this, EventArgs.Empty);
    }

    private void Run_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnRun?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetInputVector()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

        return inputVector;
    }
}
