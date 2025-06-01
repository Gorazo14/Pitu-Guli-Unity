using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
namespace GDL
{
    public class EnemyShooter : MonoBehaviour
    {
        public static event EventHandler OnAnyPlayerHit;
        public static event EventHandler OnAnyEnemyShoot;

        public static void ResetStaticData()
        {
            OnAnyPlayerHit = null;
            OnAnyEnemyShoot = null;
        }

        [Header("General")]
        public Transform shootPoint;
        public LayerMask layerMask;

        [Header("Gun")]
        public Vector3 spread = new Vector3(0.06f, 0.06f, 0.06f);
        [SerializeField] private Transform muzzleFlashEffect;

        [Header("Ammo and Reload")]
        public int shotsBeforeReload = 5;  // Public variable for shots before reload
        public float reloadTime = 3f;      // Public variable for reload time

        private EnemyReferences enemyReferences;
        private int currentShotCount = 0;  // Track how many shots have been fired
        private bool isReloading = false;  // Track if currently reloading

        private float nextTimeToFire;
        [SerializeField] private float fireRate = 3f;

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
            currentShotCount = 0;
            isReloading = false;
        }

        public void Shoot()
        {
            // Don't shoot if reloading
            if (isReloading)
                return;

            Vector3 direction = GetDirection();
            if (Physics.Raycast(shootPoint.position, direction, out RaycastHit hit, float.MaxValue, layerMask) && Time.time >= nextTimeToFire)
            {
                if (hit.transform.GetComponent<Player>() != null)
                {
                    OnAnyPlayerHit?.Invoke(this, EventArgs.Empty);
                }
                // Increment shot counter
                currentShotCount++;
                nextTimeToFire = Time.time + 1f / fireRate;
                OnAnyEnemyShoot?.Invoke(this, EventArgs.Empty);
                Instantiate(muzzleFlashEffect, shootPoint.position, shootPoint.rotation);
            }
            // Check if we need to reload
            if (currentShotCount >= shotsBeforeReload)
            {
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            // Set reloading state
            isReloading = true;

            // Wait for reload time
            yield return new WaitForSeconds(reloadTime);

            // Reset shot counter and reloading state
            currentShotCount = 0;
            isReloading = false;

            Debug.Log("Enemy finished reloading");
        }
        /*
        private IEnumerator ShootRepeatedly()
        {
            while (true) // Infinite loop to keep shooting
            {
                if (!isReloading) // Only shoot if not reloading
                {
                    Shoot(); // Call the Shoot method
                }
                yield return new WaitForSeconds(1f); // Wait for 1 second before shooting again
            }
        }

        void Start()
        {
            StartCoroutine(ShootRepeatedly()); // Start the coroutine when the game starts
        }
        */
        void Update()
        {
            
        }

        public Transform GetShootPoint()
        {
            return shootPoint;
        }
    }
}