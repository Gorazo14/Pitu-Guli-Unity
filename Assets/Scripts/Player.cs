using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public event EventHandler OnItemPickedUp;

    [SerializeField] private GameInput gameInput;
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
    [SerializeField] private float pickupDistance;
    [SerializeField] private LayerMask pickupableLayer;
    [SerializeField] private GameObject[] items;

    private bool isPickupableInWay;
    private RaycastHit hitInfo;


    private void Start()
    {
        gameInput.OnRun += GameInput_OnRun;
        gameInput.OnRunExit += GameInput_OnRunExit;
        gameInput.OnJump += GameInput_OnJump;
        gameInput.OnPickUp += GameInput_OnPickUp;

        pickUpText.SetActive(false);
        isPickupableInWay = false;
    }
    private void GameInput_OnPickUp(object sender, System.EventArgs e)
    {
        if (isPickupableInWay)
        {
            pickUpText.SetActive(false);
            hitInfo.transform.gameObject.SetActive(false);

            OnItemPickedUp?.Invoke(this, EventArgs.Empty);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandlePickup();
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
            Invoke("SetIsJumpingFalse", 1f);
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

        Vector3 moveDir = transform.right * x + transform.forward * z;

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
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, pickupDistance, pickupableLayer))
        {
            pickUpText.SetActive(true);
            isPickupableInWay = true;
        }
        else
        {
            pickUpText.SetActive(false);
            isPickupableInWay = false;
        }
    }

}
