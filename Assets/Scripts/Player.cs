using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public static Player Instance { get; private set; }

    public event EventHandler<OnItemPickedUpEventArgs> OnItemPickedUp;

    public class OnItemPickedUpEventArgs : EventArgs
    {
        public PickUp pickUp;
    }

    [SerializeField] private CharacterController controller;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;

    [SerializeField] private float speed;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float sphereRadius = 0.4f;
    [SerializeField] private LayerMask groundMask; 

    private Vector3 velocity;
    [HideInInspector] public float x;
    [HideInInspector] public float z;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isRunning;

    private bool isGrounded;

    [SerializeField] private GameObject pickUpText;
    [SerializeField] private GameObject healText;
    [SerializeField] private float pickupDistance;
    [SerializeField] private LayerMask pickupableLayer;

    private bool isPickupableInWay;
    private RaycastHit hitInfo;

    private bool isHoldingInteract;
    private float interactTimer;
    private float interactTimerMax = 3f;

    private Vector3 moveDir;
    private void Awake()
    {
        Instance = this;

        interactTimer = interactTimerMax;
    }
    private void Start()
    {
        GameInput.Instance.OnRun += GameInput_OnRun;
        GameInput.Instance.OnRunExit += GameInput_OnRunExit;
        GameInput.Instance.OnJump += GameInput_OnJump;
        GameInput.Instance.OnPickUp += GameInput_OnPickUp;
        GameInput.Instance.OnInteractPerformed += GameInput_OnInteractPerformed;
        GameInput.Instance.OnInteractCanceled += GameInput_OnInteractCanceled;

        pickUpText.SetActive(false);
        healText.SetActive(false);
        isPickupableInWay = false;
    }

    private void GameInput_OnInteractCanceled(object sender, EventArgs e)
    {
        isHoldingInteract = false;
    }

    private void GameInput_OnInteractPerformed(object sender, EventArgs e)
    {
        isHoldingInteract = true;
    }

    private void GameInput_OnPickUp(object sender, System.EventArgs e)
    {
        if (isPickupableInWay && InventoryUI.Instance.IsSpaceLeft())
        {
            pickUpText.SetActive(false);
            healText.SetActive(false);
            Destroy(hitInfo.transform.gameObject);
            OnItemPickedUp?.Invoke(this, new OnItemPickedUpEventArgs
            {
                pickUp = hitInfo.transform.gameObject.GetComponent<PickUp>()
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandlePickup();

        if (isHoldingInteract && isPickupableInWay && GetComponent<PlayerHealth>().GetPlayerHealth() < GetComponent<PlayerHealth>().GetPlayerMaxHealth() && hitInfo.transform.GetComponent<PickUp>().GetPickUpSO().isMedkit)
        {
            interactTimer -= Time.deltaTime;
            if (interactTimer <= 0f)
            {
                interactTimer = interactTimerMax;
                GetComponent<PlayerHealth>().SetPlayerHealth();
                Destroy(hitInfo.transform.gameObject);
            }
        }
        if (!isHoldingInteract || !isPickupableInWay)
        {
            interactTimer = interactTimerMax;
        }
    }

    private void GameInput_OnRun(object sender, System.EventArgs e)
    {
        isRunning = true;
    }
    private void GameInput_OnRunExit(object sender, System.EventArgs e)
    {
        isRunning = false;
    }
    private void GameInput_OnJump(object sender, System.EventArgs e)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
            Invoke(nameof(SetIsJumpingFalse), 1f);
        }
    }
    private void SetIsJumpingFalse()
    {
        isJumping = false;
    }
    private void HandleMovement()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        moveDir = transform.right * x + transform.forward * z;

        if (isRunning)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        isGrounded = Physics.CheckSphere(transform.position, sphereRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        controller.Move(moveDir * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
    private void HandlePickup()
    {
        if (Gun.Instance.GetBulletCount() >= Gun.Instance.GetMaxBullets()) return;

        float addHeight = 0.2f;
        if (Physics.Raycast(transform.position + new Vector3(0f, addHeight, 0f), transform.forward, out hitInfo, pickupDistance, pickupableLayer))
        {
            pickUpText.SetActive(true);
            if (hitInfo.transform.GetComponent<PickUp>().GetPickUpSO().isMedkit)
            healText.SetActive(true);
            isPickupableInWay = true;
        }
        else
        {
            pickUpText.SetActive(false);
            healText.SetActive(false);
            isPickupableInWay = false;
        }
    }
    public bool IsPlayerMoving()
    {
        return moveDir.magnitude > 0; 
    }

}
