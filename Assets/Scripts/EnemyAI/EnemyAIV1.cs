using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
namespace GDL
{
    public class enemyAIv1 : MonoBehaviour
    {
        public Transform target;
        public EnemyShooter enemyShooter;
        private EnemyReferences enemyReferences;
        private float pathUpdateDeadline;
        private float shootingDistance;

        [Header("Line of Sight Settings")]
        public float viewDistance = 15f;  
        public float viewAngle = 90f;     
        public LayerMask obstacleMask;    

        private bool targetInSight = false;

        private void Awake()
        {
            enemyReferences = GetComponent<EnemyReferences>();
        }

        void Start()
        {
            shootingDistance = enemyReferences.navMeshagent.stoppingDistance;
            transform.GetComponent<NetworkObject>().Spawn(true);
        }

        void Update()
        {
            if (target != null)
            {
                
                targetInSight = CheckLineOfSight();

                if (targetInSight)
                {
                    bool inRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;
                    if (inRange)
                    {
                        LookAtTarget();
                        // enemyShooter.Shoot();
                    }
                    else
                    {
                        UpdatePath();
                    }
                }
                else
                {
                    
                    enemyReferences.navMeshagent.ResetPath();
                }
            }
        }

        private bool CheckLineOfSight()
        {
            if (target == null)
                return false;

            
            Vector3 directionToTarget = target.position - transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget > viewDistance)
                return false;

            
            float angle = Vector3.Angle(transform.forward, directionToTarget.normalized);
            if (angle > viewAngle / 2f)
                return false;

            
            if (Physics.Raycast(transform.position, directionToTarget.normalized, distanceToTarget, obstacleMask))
                return false;

            return true;
        }

        private void LookAtTarget()
        {
            Vector3 LookPos = target.position - transform.position;
            LookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(LookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        }

        private void UpdatePath()
        {
            if (Time.time >= pathUpdateDeadline)
            {
                Debug.Log("Updating Path");
                pathUpdateDeadline = Time.time + enemyReferences.pathUpdatedelay;
                enemyReferences.navMeshagent.SetDestination(target.position);
            }
        }

        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, viewDistance);

            Vector3 viewAngleLeft = Quaternion.AngleAxis(-viewAngle / 2f, Vector3.up) * transform.forward;
            Vector3 viewAngleRight = Quaternion.AngleAxis(viewAngle / 2f, Vector3.up) * transform.forward;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, viewAngleLeft * viewDistance);
            Gizmos.DrawRay(transform.position, viewAngleRight * viewDistance);
        }
    }
}