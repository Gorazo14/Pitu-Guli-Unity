using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BlendTree_2D : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabsList networkPrefabsList;
    private NetworkPrefab gameInputPrefab;
    private GameInput gameInput;

    private Animator animator;

    private float velocityX = 0.0f;
    private float velocityZ = 0.0f;

    [SerializeField] private PlayerMovment playerMovement;
    [SerializeField] private GameObject gun;
    [SerializeField] private Rig handsRig;

    
    private void Awake()
    {
        gameInputPrefab = networkPrefabsList.PrefabList[0];
        gameInput = Instantiate(gameInputPrefab.Prefab).GetComponent<GameInput>();
        handsRig.weight = 0f;
    }
    private void Start()
    {
        gun.SetActive(false);
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (!IsOwner) return;

        float multiplier = 4f;
        if (playerMovement.isRunning)
        {
            velocityZ = playerMovement.z * multiplier;
            velocityX = playerMovement.x * multiplier;
        }else
        {
            velocityZ = playerMovement.z;
            velocityX = playerMovement.x;
        }
        if (playerMovement.isJumping)
        {
            animator.SetBool("isJumping", true);
        }else
        {
            animator.SetBool("isJumping", false);
        }

        gameInput.OnEquip += GameInput_OnEquip;
        gameInput.OnUnequip += GameInput_OnUnequip;

        animator.SetFloat("Velocity Z", velocityZ);
        animator.SetFloat("Velocity X", velocityX);
    }

    private void GameInput_OnUnequip(object sender, System.EventArgs e)
    {
        gun.SetActive(false);
        handsRig.weight = 0f;
    }

    private void GameInput_OnEquip(object sender, System.EventArgs e)
    {
        gun.SetActive(true);
        handsRig.weight = 1f;
    }

   
}
