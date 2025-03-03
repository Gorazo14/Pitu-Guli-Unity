using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GDL
{
    public class EnemyShooter : MonoBehaviour
    {
        [Header("General")]
        public Transform shootPoint;
        public Transform gunPoint;
        public LayerMask layerMask;

        [Header("Gun")]
        public Vector3 spread = new Vector3(0.06f, 0.06f, 0.06f);
        public TrailRenderer bulletTrail;
        private EnemyReferences enemyReferences;

        private Vector3 GetDirection()
        {
            Vector3 direction = transform.forward;
            direction += new Vector3(
                    Random.Range(-spread.x, spread.x),
                    Random.Range(-spread.y, spread.y),
                    Random.Range(-spread.z, spread.z)
                );
            direction.Normalize();
            return direction;
        }

        private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
        {
            float time = 0f;
            Vector3 startPosition = trail.transform.position;

            while (time < 1f)
            {
                trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
                time += Time.deltaTime / trail.time;

                yield return null;
            }

            trail.transform.position = hit.point;

            Destroy(trail.gameObject, trail.time);
        }

        void Awake()
        {
            enemyReferences = GetComponent<EnemyReferences>();
        }

        public void Shoot()
        {
            Vector3 direction = GetDirection();
            if (Physics.Raycast(shootPoint.position, direction, out RaycastHit hit, float.MaxValue, layerMask))
            {
                // Draw the debug raycast line with a duration matching the bullet trail's lifetime
                Debug.DrawLine(shootPoint.position, hit.point, Color.red, bulletTrail.time);

                // Spawn bullet trail
                TrailRenderer trail = Instantiate(bulletTrail, shootPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
            }
        }

        private IEnumerator ShootRepeatedly()
        {
            while (true) // Infinite loop to keep shooting
            {
                Shoot(); // Call the Shoot method
                yield return new WaitForSeconds(1f); // Wait for 1 second before shooting again
            }
        }

        void Start()
        {
            StartCoroutine(ShootRepeatedly()); // Start the coroutine when the game starts
        }

        void Update()
        {
            // You can add other update logic here if needed
        }
    }
}