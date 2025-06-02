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
        GameInput.Instance.OnShoot += GameInput_OnShoot;

        bulletCount = 30;
        BulletChangeEvent(bulletCount, magazineBulletCount);
    }

    private void GameInput_OnShoot(object sender, EventArgs e)
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
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

                BulletChangeEvent(bulletCount, magazineBulletCount);
            }
            else
            {
                bulletCount = maxBullets;
                bulletCount -= maxMagazineBullets - magazineBulletCount;
                magazineBulletCount = maxMagazineBullets;

                BulletChangeEvent(bulletCount, magazineBulletCount);
            }
        }
    }

    private void Update()
    {
        if (player.enabled == false) return;
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
        BulletChangeEvent(bulletCount, magazineBulletCount);
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

            BulletChangeEvent(bulletCount, magazineBulletCount);
        }
        else
        {
            magazineBulletCount = bulletCount;
            bulletCount = 0;

            BulletChangeEvent(bulletCount, magazineBulletCount);
        }
        isReloading = false;
    }

    private void BulletChangeEvent(int maxBullets, int magazineBullets)
    {
        OnAnyBulletChange?.Invoke(this, new OnAnyBulletChangeEventArgs
        {
            maxBullets = maxBullets,
            magazineBullets = magazineBullets
        });
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