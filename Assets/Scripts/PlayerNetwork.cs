using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform;
    [SerializeField] private Animator playerAnimator;

    private void Update()       
    {
        if (!IsOwner) return;

        Vector3 moveDir = new(0f, 0f, 0f);

        // Changing the values of the network variable
        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(spawnedObjectTransform.gameObject);
        }


        if (Input.GetKey(KeyCode.W))
        {
            moveDir.z += 1f;
            playerAnimator.SetBool("isWalking", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir.z -= 1f;
            playerAnimator.SetBool("isWalking", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x += 1f;
            playerAnimator.SetBool("isWalking", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x -= 1f;
            playerAnimator.SetBool("isWalking", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnimator.SetBool("isWalking", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnimator.SetBool("isWalking", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            playerAnimator.SetBool("isWalking", false);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            playerAnimator.SetBool("isWalking", false);
        }
        float speed = 3f;

        transform.position += speed * Time.deltaTime * moveDir.normalized;
    }
}
