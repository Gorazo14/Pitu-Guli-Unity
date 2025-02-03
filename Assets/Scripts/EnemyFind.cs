using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace GDL
{
    public class EnemyFind : MonoBehaviour
    {
        public EnemyShooter enemyShooter;
        private EnemyReferences enemyReferences;
        private float pathUpdateDeadline;
        private float shootingDistance;
        private Transform target;

        private void Awake()
        {
            enemyReferences = GetComponent<EnemyReferences>();
        }

        void Start()
        {
            shootingDistance = enemyReferences.navMeshagent.stoppingDistance;
            transform.GetComponent<NetworkObject>().Spawn(true);
            FindClosestPlayer(); 
        }

        void Update()
        {
            
            FindClosestPlayer();

            
            if (target == null) return;

           
            bool inRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;

            if (inRange)
            {
                LookAtTarget();
                enemyShooter.Shoot();
            }
            else
            {
                UpdatePath();
            }
        }

        private void FindClosestPlayer()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            float closestDistance = Mathf.Infinity;
            Transform closestPlayer = null;

            
            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player.transform;
                }
            }

           
            target = closestPlayer;
        }

        private void LookAtTarget()
        {
            if (target == null) return;

            
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0; 
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        }

        private void UpdatePath()
        {
            if (target == null) return;

            
            if (Time.time >= pathUpdateDeadline)
            {
                Debug.Log("Updating Path");
                pathUpdateDeadline = Time.time + enemyReferences.pathUpdatedelay;
                enemyReferences.navMeshagent.SetDestination(target.position);
            }
        }
    }
}