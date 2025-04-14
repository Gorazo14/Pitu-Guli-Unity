using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private PickUpSO pickUpSO;

    public PickUpSO GetPickUpSO()
    {
        return pickUpSO;
    }
}
