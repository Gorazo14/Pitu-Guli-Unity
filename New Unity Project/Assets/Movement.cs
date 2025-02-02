using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Specialized;

public class Movement : NetworkBehaviour
{
    private Vector3 direction = new Vector3(0f, 0f, 0f);
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKey(KeyCode.W))
        {
            direction.z += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.z -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1f;
        }
    }
}
