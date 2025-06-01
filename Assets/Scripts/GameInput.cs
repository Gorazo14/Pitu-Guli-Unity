using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    public event EventHandler OnJump;
    public event EventHandler OnRun;
    public event EventHandler OnRunExit;
    public event EventHandler OnGunEquip;
    public event EventHandler OnPickUp;
    public event EventHandler OnInventoryOpenClose;
    public event EventHandler OnReload;
    public event EventHandler OnInteractPerformed;
    public event EventHandler OnInteractCanceled;
    public event EventHandler OnShoot;
    public event EventHandler OnPause;
    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.Run.performed += Run_performed;
        playerInputActions.Player.Run.canceled += Run_canceled;
        playerInputActions.Gun.EquipGun.performed += EquipGun_performed;
        playerInputActions.Player.PickUp.performed += PickUp_performed;
        playerInputActions.Player.Inventory.performed += Inventory_performed;
        playerInputActions.Gun.Reload.performed += Reload_performed;
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Interact.canceled += Interact_canceled;
        playerInputActions.Gun.Shoot.performed += Shoot_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }
    private void OnDestroy()
    {
        playerInputActions.Dispose();

        playerInputActions.Player.Jump.performed -= Jump_performed;
        playerInputActions.Player.Run.performed -= Run_performed;
        playerInputActions.Player.Run.canceled -= Run_canceled;
        playerInputActions.Gun.EquipGun.performed -= EquipGun_performed;
        playerInputActions.Player.PickUp.performed -= PickUp_performed;
        playerInputActions.Player.Inventory.performed -= Inventory_performed;
        playerInputActions.Gun.Reload.performed -= Reload_performed;
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.Interact.canceled -= Interact_canceled;
        playerInputActions.Gun.Shoot.performed -= Shoot_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShoot?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Reload_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnReload?.Invoke(this, EventArgs.Empty);
    }

    private void EquipGun_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnGunEquip?.Invoke(this, EventArgs.Empty);
    }

    private void Inventory_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInventoryOpenClose?.Invoke(this, EventArgs.Empty);
    }

    private void PickUp_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPickUp?.Invoke(this, EventArgs.Empty);
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
