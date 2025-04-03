using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PickUpSO : ScriptableObject
{
    public string pickUpName;
    public bool isStackable;
    public int maxStack;
}
