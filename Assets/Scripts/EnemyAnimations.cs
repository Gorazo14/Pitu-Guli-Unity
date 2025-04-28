using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private const string IS_RUNNING = "IsRunning";
    private const string IS_PLAYER_IN_RANGE = "IsPlayerInRange";

    private Animator animator;
    [SerializeField] private EnemyFind enemyFind;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, enemyFind.IsEnemyMoving());
    }
}
