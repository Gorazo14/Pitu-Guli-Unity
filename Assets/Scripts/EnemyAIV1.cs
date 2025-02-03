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
            if(Time.time >= pathUpdateDeadline)
            {
                Debug.Log("Updating Path");
                pathUpdateDeadline = Time.time + enemyReferences.pathUpdatedelay;
                enemyReferences.navMeshagent.SetDestination(target.position);
            }
        }
    }
}
