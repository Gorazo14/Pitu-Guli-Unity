using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovment : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabsList networkPrefabsList;
    private NetworkPrefab gameInputPrefab;
    private GameInput gameInput;

    public CharacterController controller;

    [SerializeField] private float walkSpeed = 2f;
    public float speed;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    [SerializeField] private float runSpeed = 4f;

    public float sphereRadius = 0.4f;
    public LayerMask groundMask; 

    Vector3 velocity;
    [HideInInspector] public float x;
    [HideInInspector] public float z;
    [HideInInspector] public bool isJumping;

    [HideInInspector] public bool isRunning;
    private bool isGrounded;

    
    private void Awake()
    {
        gameInputPrefab = networkPrefabsList.PrefabList[0];
        gameInput = Instantiate(gameInputPrefab.Prefab).GetComponent<GameInput>();
    }
    private void Start()
    {
        gameInputPrefab = networkPrefabsList.PrefabList[0];
        gameInput = Instantiate(gameInputPrefab.Prefab).GetComponent<GameInput>();
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.right * x + transform.forward * z;

        gameInput.OnRun += GameInput_OnRun;
        gameInput.OnRunExit += GameInput_OnRunExit;

        if (isRunning)
        {
            speed = runSpeed;
        }else
        {
            speed = walkSpeed;
        }

        isGrounded = Physics.CheckSphere(transform.position, sphereRadius, groundMask);
        
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        controller.Move(moveDir * speed * Time.deltaTime);

        gameInput.OnJump += GameInput_OnJump;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
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

}
