using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PickUpSO : ScriptableObject
{
    public string pickUpName;
    public bool isStackable;
    public int maxStack;
    public Transform pickUpPrefab;
    public Sprite pickUpSprite;
    public bool isMedkit;
    public int bulletCount;
    public bool isAmmo;
}
