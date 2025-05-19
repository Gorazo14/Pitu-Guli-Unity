using System;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    public static Gun Instance { get; private set; }

    public static event EventHandler OnAnyShoot;
    public static event EventHandler<OnAnyBulletChangeEventArgs> OnAnyBulletChange;
    public class OnAnyBulletChangeEventArgs : EventArgs
    {
        public int magazineBullets;
        public int maxBullets;
    }

    [SerializeField] private Camera fpsCamera;
    [SerializeField] private Transform shootParticleEffect;

    [Header("Shooting")]
    [SerializeField] private Player player;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform gunTip;

    [Header("BulletCounting")]
    [SerializeField] private int maxBullets = 100;
    private int bulletCount = 0;
    [SerializeField] private int maxMagazineBullets = 1;
    private int magazineBulletCount = 0;
    [SerializeField] private float reloadTime = 2f;
    private bool isReloading = false;

    private float nextTimeToFire = 0f;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Player.Instance.OnItemPickedUp += Player_OnItemPickedUp;
        GameInput.Instance.OnReload += GameInput_OnReload;
    }

    private void GameInput_OnReload(object sender, EventArgs e)
    {
        if (!isReloading && magazineBulletCount < maxMagazineBullets)
        Invoke(nameof(Reload), reloadTime);
    }

    private void Player_OnItemPickedUp(object sender, Player.OnItemPickedUpEventArgs e)
    {
        if (e.pickUp.GetPickUpSO().isAmmo)
        {
            AmmoBox ammoBox = e.pickUp.GetComponent<AmmoBox>();
            if (bulletCount + ammoBox.GetBulletCount() < maxBullets)
            {
                bulletCount += ammoBox.GetBulletCount();
                bulletCount -= maxMagazineBullets - magazineBulletCount;
                magazineBulletCount = maxMagazineBullets;

                OnAnyBulletChange?.Invoke(this, new OnAnyBulletChangeEventArgs
                {
                    maxBullets = bulletCount,
                    magazineBullets = magazineBulletCount
                });
            }else
            {
                bulletCount = maxBullets;
                bulletCount -= maxMagazineBullets - magazineBulletCount;
                magazineBulletCount = maxMagazineBullets;


                OnAnyBulletChange?.Invoke(this, new OnAnyBulletChangeEventArgs
                {
                    maxBullets = bulletCount,
                    magazineBullets = magazineBulletCount
                });
            }
        }
    }

    private void Update()
    {
        if (player.enabled == false)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (magazineBulletCount <= 0 || isReloading) return;
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        Transform shootParticleEffectTransform = Instantiate(shootParticleEffect, gunTip.position, gunTip.rotation);
        OnAnyShoot?.Invoke(this, EventArgs.Empty);

        magazineBulletCount--;
        OnAnyBulletChange?.Invoke(this, new OnAnyBulletChangeEventArgs
        {
            maxBullets = bulletCount,
            magazineBullets = magazineBulletCount
        });
        if (magazineBulletCount <= 0)
        {
            isReloading = true;
            Invoke(nameof(Reload), reloadTime);
        }
    }

    private void Reload()
    {
        if (bulletCount >= maxMagazineBullets - magazineBulletCount)
        {
            bulletCount -= maxMagazineBullets - magazineBulletCount;
            magazineBulletCount = maxMagazineBullets;

            OnAnyBulletChange?.Invoke(this, new OnAnyBulletChangeEventArgs
            {
                maxBullets = bulletCount,
                magazineBullets = magazineBulletCount
            });
        }
        else
        {
            magazineBulletCount = bulletCount;
            bulletCount = 0;

            OnAnyBulletChange?.Invoke(this, new OnAnyBulletChangeEventArgs
            {
                maxBullets = bulletCount,
                magazineBullets = magazineBulletCount
            });
        }
        isReloading = false;
    }
    public Transform GetGunTip()
    {
        return gunTip;
    }
    public int GetBulletCount()
    {
        return bulletCount;
    }
    public int GetMaxBullets()
    {
        return maxBullets;
    }
}