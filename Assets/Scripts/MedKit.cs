using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    [SerializeField] private PickUpSO pickUpSO;

    public PickUpSO GetPickUpSO ()
    {
        return pickUpSO;
    }
}
