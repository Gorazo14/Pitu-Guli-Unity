using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAnimations : MonoBehaviour
{
    private const string IS_RUNNING = "IsRunning";
    private const string IS_PLAYER_IN_RANGE = "IsPlayerInRange";
    private const string IS_ENEMY_DEAD = "EnemyDeath";

    private Animator animator;
    [SerializeField] private EnemyFind enemyFind;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        transform.parent.GetComponent<Target>().OnEnemyDeath += Target_OnEnemyDeath;
    }

    private void Target_OnEnemyDeath(object sender, System.EventArgs e)
    {
        animator.SetTrigger(IS_ENEMY_DEAD);
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, enemyFind.IsEnemyMoving());
    }
}
