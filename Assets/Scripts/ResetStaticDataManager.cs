using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        EnemyShooter.ResetStaticData();
    }
}
