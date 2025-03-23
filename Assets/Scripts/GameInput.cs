using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnJump;
    public event EventHandler OnRun;
    public event EventHandler OnRunExit;
    public event EventHandler OnEquip;
    public event EventHandler OnUnequip;
    public event EventHandler OnPickUp;
    public event EventHandler OnInventoryOpenClose;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.Run.performed += Run_performed;
        playerInputActions.Player.Run.canceled += Run_canceled;
        playerInputActions.Gun.Equip.performed += Equip_performed;
        playerInputActions.Gun.Unequip.performed += Unequip_performed;
        playerInputActions.Player.PickUp.performed += PickUp_performed;
        playerInputActions.Player.Inventory.performed += Inventory_performed;
    }

    private void Inventory_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInventoryOpenClose?.Invoke(this, EventArgs.Empty);
    }

    private void PickUp_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPickUp?.Invoke(this, EventArgs.Empty);
    }

    private void Unequip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnUnequip?.Invoke(this, EventArgs.Empty);
    }

    private void Equip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnEquip?.Invoke(this, EventArgs.Empty);
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
}
