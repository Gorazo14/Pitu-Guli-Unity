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
        [Header("Line of Sight")]
        public LayerMask obstacleLayerMask; 
        public float visionCheckInterval = 0.2f; 
        private float nextVisionCheck;
        private bool isInRange;

        private void Awake()
        {
            enemyReferences = GetComponent<EnemyReferences>();
        }

        void Start()
        {
            shootingDistance = enemyReferences.navMeshagent.stoppingDistance;
            transform.GetComponent<NetworkObject>().Spawn(true);
            FindPlayerWithLineOfSight();
        }

        void Update()
        {
            if (Time.time >= nextVisionCheck)
            {
                nextVisionCheck = Time.time + visionCheckInterval;
                FindPlayerWithLineOfSight();
            }

            if (target == null) return;

            isInRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;
            if (isInRange)
            {
                LookAtTarget();
                enemyShooter.Shoot();
            }
            else
            {
                UpdatePath();
            }
        }

        private void FindPlayerWithLineOfSight()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            float closestDistance = Mathf.Infinity;
            Transform closestVisiblePlayer = null;

            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                
                if (distance < closestDistance)
                {
                    
                    if (HasLineOfSightTo(player.transform))
                    {
                        closestDistance = distance;
                        closestVisiblePlayer = player.transform;
                    }
                }
            }

            
            if (closestVisiblePlayer != null)
            {
                target = closestVisiblePlayer;
            }
            else if (target != null)
            {
                
                if (!HasLineOfSightTo(target))
                {
                    target = null; 
                }
            }
        }

        private bool HasLineOfSightTo(Transform targetTransform)
        {
            if (targetTransform == null) return false;

            Vector3 directionToTarget = targetTransform.position - transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            
            if (Physics.Raycast(transform.position, directionToTarget.normalized, out RaycastHit hit, distanceToTarget, obstacleLayerMask))
            {
                
                if (hit.transform != targetTransform)
                {
                    return false;
                }
            }

            return true; 
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

        public bool IsEnemyMoving()
        {
            return enemyReferences.navMeshagent.velocity.magnitude != 0;
        }
        public bool IsPlayerInRange()
        {
            return isInRange;
        }
    }
}