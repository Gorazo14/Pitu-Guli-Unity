using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerHead;

    private void Update()
    {
        transform.position = playerHead.position;
    }
}
