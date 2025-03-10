using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    private Animator animator;

    private float velocityX = 0.0f;
    private float velocityZ = 0.0f;

    [SerializeField] private Player player;
    [SerializeField] private GameObject gun;
    [SerializeField] private Rig handsRig;
    [SerializeField] private Rig chainRig;

    
    private void Awake()
    {
        handsRig.weight = 0f;
        chainRig.weight = 0f;   
    }
    private void Start()
    {
        gun.SetActive(false);
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (player.enabled == false)
        {
            return;
        }

        float multiplier = 4f;
        if (player.isRunning)
        {
            velocityZ = player.z * multiplier;
            velocityX = player.x * multiplier;
        }else
        {
            velocityZ = player.z;
            velocityX = player.x;
        }
        if (player.isJumping)
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
        chainRig.weight = 0f;
    }

    private void GameInput_OnEquip(object sender, System.EventArgs e)
    {
        gun.SetActive(true);
        handsRig.weight = 1f;
        chainRig.weight = 1f;
    }

   
}
